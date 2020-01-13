using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public interface IBaseRepository<T>
    {
        Task AddAsync(T instance);

        Task SaveChangesAsync();
    }
}
