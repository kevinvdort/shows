using MediatR;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetShowAndCastQuery : IRequest<GetShowAndCastModel>
    {
        public int Page { get; set; }

        public int Size { get; set; }
    }
}
