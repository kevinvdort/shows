using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public interface ITvMazeShowCastRepository : IBaseRepository<TvMazeShowCast>
    {
        Task<IEnumerable<TvMazeShowCast>> GetByShowIdAsync(int showId);
    }
}
