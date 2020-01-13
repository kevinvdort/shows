using AutoMapper;
using RTL.TvMaze.Domain.TvMaze.Models;
using RTL.TvMaze.Infrastructure.Entities;
using System.Linq;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetShowAndCastProfile : Profile
    {
        public GetShowAndCastProfile()
        {
            CreateMap<TvMazeShow, TvMazeShowAndCastModel>()
                .ForMember(m => m.Show, config =>
                    config.MapFrom(src =>
                        new TvMazeShowModel
                        {
                            Id = src.Id,
                            Name = src.Name
                        }))
                .ForMember(m => m.Cast,
                    config => config.MapFrom(src =>
                        src.TvMazeShowCast.Select(c =>
                            new TvMazePersonModel
                            {
                                Id = c.TvMazePerson.Id,
                                Name = c.TvMazePerson.Name,
                                Birthday = c.TvMazePerson.Birthday
                            })));

        }
    }
}
