using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RTL.TvMaze.Domain.TvMaze.Models;
using RTL.TvMaze.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetShowAndCastQueryHandler : IRequestHandler<GetShowAndCastQuery, GetShowAndCastModel>
    {
        private readonly ILogger<GetShowAndCastQueryHandler> logger;
        private readonly IMapper mapper;
        private readonly ITvMazeShowRepository tvMazeShowRepository;

        public GetShowAndCastQueryHandler(ILogger<GetShowAndCastQueryHandler> logger,
                                          IMapper mapper,
                                          ITvMazeShowRepository tvMazeShowRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.tvMazeShowRepository = tvMazeShowRepository;
        }

        public async Task<GetShowAndCastModel> Handle(GetShowAndCastQuery request, CancellationToken cancellationToken)
        {
            var getShowAndCastModel = new GetShowAndCastModel();

            var result = await tvMazeShowRepository.GetPage(request.Page, request.Size);

            getShowAndCastModel.Shows = mapper.Map<IEnumerable<TvMazeShowAndCastModel>>(result);

            return getShowAndCastModel;
        }
    }
}
