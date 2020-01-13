using RTL.TvMaze.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Services
{
    public interface IHttpTvMazeService
    {
        Task<IEnumerable<TvMazeApiShowModel>> DownloadShowIndex();

        Task<IEnumerable<TvMazeApiCastModel>> DownloadCastFromShows(IEnumerable<int> shows);
    }
}
