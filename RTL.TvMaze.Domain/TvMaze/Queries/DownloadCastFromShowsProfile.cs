using AutoMapper;
using RTL.TvMaze.Domain.TvMaze.Models;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadCastFromShowsProfile : Profile
    {
        public DownloadCastFromShowsProfile()
        {
            CreateMap<Infrastructure.Models.TvMazeApiPersonModel, TvMazePersonModel>();
        }
    }
}
