using Microsoft.EntityFrameworkCore;
using RTL.TvMaze.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public class TvMazeShowRepository : BaseRepository<TvMazeShow>, ITvMazeShowRepository
    {
        public TvMazeShowRepository(RTLDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<TvMazeShow> GetByTvMazeIdAsync(int id)
        {
            return await DbContext.TvMazeShow.FirstOrDefaultAsync(i => i.TvMazeId == id);
        }

        public async Task<IEnumerable<TvMazeShow>> GetPage(int page, int size)
        {
            return await DbContext.TvMazeShow.Include(x => x.TvMazeShowCast)
                                             .ThenInclude(s => s.TvMazePerson)
                                             .Skip((page - 1) * size)
                                             .Take(size)
                                             .ToListAsync();
        }
    }
}
