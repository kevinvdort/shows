using MediatR;
using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadCastFromShowsQuery : IRequest<DownloadCastFromShowsModel>
    {
        public IEnumerable<int> Shows { get; set; }
    }
}
