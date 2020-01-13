using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RTL.TvMaze.Api.Shows.Models;
using RTL.TvMaze.Domain.TvMaze.Models;
using RTL.TvMaze.Domain.TvMaze.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TvMaze.Api.Shows.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ShowsController(IMediator mediator,
                               IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShowModel>))]
        public async Task<ActionResult> Shows(int page, int size)
        {
            var getShowAndCastQueryResult = await mediator.Send(new GetShowAndCastQuery
            {
                Page = page,
                Size = size
            });

            var showModel = mapper.Map<IEnumerable<ShowModel>>(getShowAndCastQueryResult.Shows);

            return new JsonResult(showModel);
        }
    }
}
