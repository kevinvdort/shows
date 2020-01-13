using System;

namespace RTL.TvMaze.Infrastructure.Entities
{
    public partial class TvMazeShowIndex
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public long Duration { get; set; }
        public bool InProgress { get; set; }
        public int RecordsCreated { get; set; }
        public int RecordsUpdated { get; set; }
    }
}
