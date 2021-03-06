﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RTL.TvMaze.Infrastructure.Models;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using RTL.TvMaze.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RTL.TvMaze.Infrastructure.Extensions;
using RTL.TvMaze.Infrastructure.HttpClient;

namespace RTL.TvMaze.Infrastructure.Services
{
    public class HttpTvMazeApiService : IHttpTvMazeApiService
    {
        private readonly IOptions<TvMazeApiSettings> tvMazeApiSettings;
        private readonly TvMazeApiHttpClient tvMazeApiHttpClient;
        private readonly ILogger<HttpTvMazeApiService> logger;

        public HttpTvMazeApiService(IOptions<TvMazeApiSettings> tvMazeApiSettings,
                                    TvMazeApiHttpClient tvMazeApiHttpClient,
                                    ILogger<HttpTvMazeApiService> logger)
        {
            this.tvMazeApiSettings = tvMazeApiSettings;
            this.tvMazeApiHttpClient = tvMazeApiHttpClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<TvMazeApiShowModel>> DownloadShowIndex()
        {
            bool tvMazeFullIndex = false;
            int pageNumber = 0;
            var tvMazeData = new List<TvMazeApiShowModel>();

            while (!tvMazeFullIndex)
            {
                using (var response = await tvMazeApiHttpClient.HttpClient.GetAsync($"shows?page={pageNumber}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var singleSetTvMazeData = JsonConvert.DeserializeObject<List<TvMazeApiShowModel>>(apiResponse);

                    if (singleSetTvMazeData.Any())
                    {
                        tvMazeData.AddRange(singleSetTvMazeData);
                        pageNumber++;
                    }
                    else
                    {
                        tvMazeFullIndex = true;
                    }
                }

#if DEBUG
                // Debug purposses only!
                if (pageNumber == 1)
                {
                    break;
                }
#endif
            }

            return tvMazeData;
        }

        public async Task<IEnumerable<TvMazeApiCastModel>> DownloadCastFromShows(IEnumerable<int> shows)
        {
            var tvMazeApiCastModels = new List<TvMazeApiCastModel>();

            // According to the documentation from TV Maze, at least 20 requests (/shows/id/cast) per 10 seconds.
            var showThrottledChunks = shows.ToArray().Split(20);

            foreach (var showChunk in showThrottledChunks)
            {
                foreach (var id in showChunk)
                {
                    using var response = await tvMazeApiHttpClient.HttpClient.GetAsync($"shows/{id}/cast");
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var tvMazeApiPeopleModelCollection = JsonConvert.DeserializeObject<List<TvMazeApiCastModel>>(apiResponse);

                        foreach (var cast in tvMazeApiPeopleModelCollection)
                        {
                            cast.ShowId = id;
                            tvMazeApiCastModels.Add(cast);
                        }
                    }
                    catch (HttpRequestException exception)
                    {
                        logger.LogError(exception, "Error downloading people");
                        throw exception;
                    }
                }

                // At least 20 calls every 10 seconds per IP address.
                await Task.Delay(10000);
            }
            return tvMazeApiCastModels;
        }
    }
}