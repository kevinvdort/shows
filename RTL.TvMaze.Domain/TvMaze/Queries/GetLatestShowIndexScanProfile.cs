using AutoMapper;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetLatestShowIndexScanProfile : Profile
    {
        public GetLatestShowIndexScanProfile()
        {
            CreateMap<Infrastructure.Entities.TvMazeShowIndex, GetLatestShowIndexScanQueryModel>();
        }
    }
}
