using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Models
{
    public class TvMazeShowAndCastModel
    {
        public TvMazeShowModel Show { get; set; }

        public IEnumerable<TvMazePersonModel> Cast { get; set; }
    }
}
