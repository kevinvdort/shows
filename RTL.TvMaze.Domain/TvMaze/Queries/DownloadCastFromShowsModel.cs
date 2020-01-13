using RTL.TvMaze.Domain.TvMaze.Enumerations;
using RTL.TvMaze.Domain.TvMaze.Models;
using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadCastFromShowsModel
    {
        public ScanStatus ScanStatus { get; set; }

        public long Duration { get; set; }

        public IList<TvMazeCastModel> TvMazePeopleModelCollection { get; set; }
    }
}
