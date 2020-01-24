using Microsoft.Extensions.Options;
using RTL.TvMaze.Infrastructure.Configurations;

namespace RTL.TvMaze.Infrastructure.HttpClient
{
    public class TvMazeApiHttpClient
    {
        private readonly IOptions<TvMazeApiSettings> tvMazeApiSettings;

        public TvMazeApiHttpClient(System.Net.Http.HttpClient httpClient,
                                   IOptions<TvMazeApiSettings> tvMazeApiSettings)
        {
            HttpClient = httpClient;
            this.tvMazeApiSettings = tvMazeApiSettings;

            httpClient.BaseAddress = tvMazeApiSettings.Value?.BaseAddress;
        }

        public System.Net.Http.HttpClient HttpClient { get; }
    }
}
