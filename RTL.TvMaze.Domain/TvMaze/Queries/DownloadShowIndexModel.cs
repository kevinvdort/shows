using RTL.TvMaze.Domain.TvMaze.Enumerations;
using RTL.TvMaze.Domain.TvMaze.Models;
using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadShowIndexModel
    {
        public ScanStatus ScanStatus { get; set; }

        public long Duration { get; set; }

        public IEnumerable<TvMazeShowModel> TvMazeShowModel { get; set; }
    }
}
