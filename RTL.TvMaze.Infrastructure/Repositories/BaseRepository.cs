using RTL.TvMaze.Infrastructure.Entities;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
    {
        public RTLDbContext DbContext { get; private set; }

        public BaseRepository(RTLDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddAsync(T instance) 
        {
            await DbContext.AddAsync(instance);
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
