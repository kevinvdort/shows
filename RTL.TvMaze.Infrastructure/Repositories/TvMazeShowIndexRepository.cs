using Microsoft.EntityFrameworkCore;
using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public class TvMazeShowIndexRepository : BaseRepository<TvMazeShowIndex>, ITvMazeShowIndexRepository
    {
        public TvMazeShowIndexRepository(RTLDbContext dbContext): base(dbContext)
        {
        }

        public async Task<IEnumerable<TvMazeShowIndex>> GetAllAsync()
        {
            return await DbContext.TvMazeShowIndex.ToListAsync();
        }

        public async Task<TvMazeShowIndex> GetLatestAsync()
        {
            return await DbContext.TvMazeShowIndex.OrderByDescending(p => p.Id)
                                                  .FirstOrDefaultAsync();
        }
    }
}
