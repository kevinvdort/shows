
using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public interface ITvMazeShowRepository : IBaseRepository<TvMazeShow>
    {
        Task<TvMazeShow> GetByTvMazeIdAsync(int id);

        Task<IEnumerable<TvMazeShow>> GetPage(int page, int size);
    }
}
