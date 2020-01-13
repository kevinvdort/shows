using MediatR;
using RTL.TvMaze.Domain.TvMaze.Models;
using System.Collections.Generic;

namespace RTL.TvMaze.Domain.TvMaze.Commands
{
    public class InsertOrUpdateShowAndCastCommand : IRequest<bool>
    {
        public long Duration { get; set; }

        public IEnumerable<TvMazeShowModel> TvMazeShowModelCollection { get; set; }

        public IEnumerable<TvMazeCastModel> TvMazeCastModelCollection { get; set; }
    }
}
