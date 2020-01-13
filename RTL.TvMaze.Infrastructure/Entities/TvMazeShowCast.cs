namespace RTL.TvMaze.Infrastructure.Entities
{
    public partial class TvMazeShowCast
    {
        public int Id { get; set; }
        public int TvMazeShowId { get; set; }
        public int TvMazePersonId { get; set; }

        public virtual TvMazePerson TvMazePerson { get; set; }
        public virtual TvMazeShow TvMazeShow { get; set; }
    }
}
