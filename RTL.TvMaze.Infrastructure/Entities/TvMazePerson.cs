using System;
using System.Collections.Generic;

namespace RTL.TvMaze.Infrastructure.Entities
{
    public partial class TvMazePerson
    {
        public TvMazePerson()
        {
            TvMazeShowCast = new HashSet<TvMazeShowCast>();
        }

        public int Id { get; set; }
        public int TvMazeId { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<TvMazeShowCast> TvMazeShowCast { get; set; }
    }
}
