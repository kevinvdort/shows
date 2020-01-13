using AutoMapper;
using MediatR;
using RTL.TvMaze.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class GetLatestShowIndexScanQueryHandler : IRequestHandler<GetLatestShowIndexScanQuery, GetLatestShowIndexScanQueryModel>
    {
        private readonly ITvMazeShowIndexRepository tvMazeShowIndexRepository;
        private readonly IMapper mapper;

        public GetLatestShowIndexScanQueryHandler(ITvMazeShowIndexRepository tvMazeShowIndexRepository,
                                                  IMapper mapper)
        {
            this.tvMazeShowIndexRepository = tvMazeShowIndexRepository;
            this.mapper = mapper;
        }

        public async Task<GetLatestShowIndexScanQueryModel> Handle(GetLatestShowIndexScanQuery request, CancellationToken cancellationToken)
        {
            var showIndexScan = await tvMazeShowIndexRepository.GetLatestAsync();
            return mapper.Map<GetLatestShowIndexScanQueryModel>(showIndexScan);
        }
    }
}
