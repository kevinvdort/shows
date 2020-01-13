using RTL.TvMaze.Domain.TvMaze.Models;
using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetShowAndCastModel
    {
        public IEnumerable<TvMazeShowAndCastModel> Shows { get; set; }
    }
}
