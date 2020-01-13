using Microsoft.EntityFrameworkCore;
using RTL.TvMaze.Infrastructure.Entities;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public class TvMazePersonRepository : BaseRepository<TvMazePerson>, ITvMazePersonRepository
    {
        public TvMazePersonRepository(RTLDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<TvMazePerson> GetByTvMazeIdAsync(int id)
        {
            return await DbContext.TvMazePerson.FirstOrDefaultAsync(i => i.TvMazeId == id);
        }
    }
}
