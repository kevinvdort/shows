using Microsoft.EntityFrameworkCore;
using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public class TvMazeShowCastRepository : BaseRepository<TvMazeShowCast>, ITvMazeShowCastRepository
    {
        public TvMazeShowCastRepository(RTLDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<TvMazeShowCast>> GetByShowIdAsync(int showId)
        {
            return await DbContext.TvMazeShowCast.Where(x => x.TvMazeShowId == showId)
                                                 .ToListAsync();
        }
    }
}
