using AutoMapper;
using RTL.TvMaze.Domain.TvMaze.Models;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadShowIndexProfile : Profile
    {
        public DownloadShowIndexProfile()
        {
            CreateMap<Infrastructure.Entities.TvMazeShow, DownloadShowIndexModel>();
            CreateMap<Infrastructure.Models.TvMazeApiShowModel, TvMazeShowModel>();
        }
    }
}
