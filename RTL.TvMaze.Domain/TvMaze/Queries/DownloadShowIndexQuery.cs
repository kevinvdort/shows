using MediatR;
using System;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadShowIndexQuery : IRequest<DownloadShowIndexModel>
    {
        public int AdditionalMinutesToLatestScan { get; set; }
        public DateTime? LatestScanTimeStamp { get; set; }
    }
}
