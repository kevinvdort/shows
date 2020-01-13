using System.Collections.Generic;

namespace RTL.TvMaze.Infrastructure.Entities
{
    public partial class TvMazeShow
    {
        public TvMazeShow()
        {
            TvMazeShowCast = new HashSet<TvMazeShowCast>();
        }

        public int Id { get; set; }
        public int TvMazeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TvMazeShowCast> TvMazeShowCast { get; set; }
    }
}
