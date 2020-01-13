using RTL.TvMaze.Infrastructure.Entities;
using System.Threading.Tasks;

namespace RTL.TvMaze.Infrastructure.Repositories
{
    public interface ITvMazePersonRepository : IBaseRepository<TvMazePerson>
    {
        Task<TvMazePerson> GetByTvMazeIdAsync(int id);
    }
}
