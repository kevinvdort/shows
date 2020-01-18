using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RTL.TvMaze.Domain.TvMaze.Enumerations;
using RTL.TvMaze.Domain.TvMaze.Models;
using RTL.TvMaze.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadCastFromShowsQueryHandler : IRequestHandler<DownloadCastFromShowsQuery, DownloadCastFromShowsModel>
    {
        private readonly IMapper mapper;
        private readonly IHttpTvMazeApiService httpScrapeService;
        private readonly ILogger<DownloadCastFromShowsQueryHandler> logger;

        public DownloadCastFromShowsQueryHandler(IMapper mapper,
                                          IHttpTvMazeApiService httpScrapeService,
                                          ILogger<DownloadCastFromShowsQueryHandler> logger)
        {
            this.mapper = mapper;
            this.httpScrapeService = httpScrapeService;
            this.logger = logger;
        }

        public async Task<DownloadCastFromShowsModel> Handle(DownloadCastFromShowsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Initiating handle");

            var downloadCastFromShowsModel = new DownloadCastFromShowsModel();

            try
            {
                logger.LogInformation("Start downloading people");

                var downloadStopWatch = Stopwatch.StartNew();

                var castCollection = await httpScrapeService.DownloadShowIndexV2();

                downloadStopWatch.Stop();

                downloadCastFromShowsModel.ScanStatus = ScanStatus.Complete;
                downloadCastFromShowsModel.TvMazePeopleModelCollection = new List<TvMazeCastModel>();

                foreach (var show in castCollection)
                {
                    downloadCastFromShowsModel.TvMazePeopleModelCollection.Add(new TvMazeCastModel
                    {
                        ShowId = show.ShowId,
                        Person = mapper.Map<TvMazePersonModel>(show.Person)
                    });
                }

                downloadCastFromShowsModel.Duration = downloadStopWatch.ElapsedMilliseconds;

                logger.LogInformation($"Downloading people complete with a duration of {downloadStopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occured");
                downloadCastFromShowsModel.ScanStatus = ScanStatus.Failed;
            }

            return downloadCastFromShowsModel;
        }
    }
}
