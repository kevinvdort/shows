using AutoMapper;
using RTL.TvMaze.Api.Shows.Models;
using RTL.TvMaze.Domain.TvMaze.Models;
using System.Linq;

namespace RTL.TvMaze.Api.Shows.Mapping
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            CreateMap<TvMazeShowAndCastModel, ShowModel>()
                .ForMember(m => m.Id, config => config.MapFrom(src => src.Show.Id))
                .ForMember(m => m.Name, config => config.MapFrom(src => src.Show.Name))
                .ForMember(m => m.Cast, config =>
                    config.MapFrom(src =>
                        src.Cast.Select(c =>
                            new PersonModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Birthday = c.Birthday
                            })));
        }
    }
}
