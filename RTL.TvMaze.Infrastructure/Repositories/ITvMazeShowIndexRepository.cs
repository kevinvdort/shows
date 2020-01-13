using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public interface ITvMazeShowIndexRepository : IBaseRepository<TvMazeShowIndex>
    {
        Task<TvMazeShowIndex> GetLatestAsync();

        Task<IEnumerable<TvMazeShowIndex>> GetAllAsync();
    }
}
