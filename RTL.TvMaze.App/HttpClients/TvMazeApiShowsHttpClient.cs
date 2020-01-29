using Microsoft.Extensions.Options;
using System.Net.Http;

namespace RTL.TvMaze.App.HttpClients
{
    public class TvMazeApiShowsHttpClient
    {
        public TvMazeApiShowsHttpClient(HttpClient httpClient,
                                        IOptions<TvMazeApiShowsHttpSettings> tvMazeApiShowsHttpSettings)
        {
            HttpClient = httpClient;
            HttpClient.BaseAddress = tvMazeApiShowsHttpSettings.Value?.BaseAddress;
        }

        public HttpClient HttpClient { get; }
    }
}
