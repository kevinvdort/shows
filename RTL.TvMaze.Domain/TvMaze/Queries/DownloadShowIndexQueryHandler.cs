using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RTL.TvMaze.Domain.TvMaze.Enumerations;
using RTL.TvMaze.Domain.TvMaze.Models;
using RTL.TvMaze.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.TvMaze.Domain.TvMaze.Queries
{
    public class DownloadShowIndexQueryHandler : IRequestHandler<DownloadShowIndexQuery, DownloadShowIndexModel>
    {
        private readonly IMapper mapper;
        private readonly IHttpTvMazeApiService httpScrapeService;
        private readonly ILogger<DownloadShowIndexQueryHandler> logger;

        public DownloadShowIndexQueryHandler(IMapper mapper,
                                            IHttpTvMazeApiService httpScrapeService,
                                            ILogger<DownloadShowIndexQueryHandler> logger)
        {
            this.mapper = mapper;
            this.httpScrapeService = httpScrapeService;
            this.logger = logger;
        }

        public async Task<DownloadShowIndexModel> Handle(DownloadShowIndexQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Initiating handle");

            var downloadShowIndexModel = new DownloadShowIndexModel();

            bool showIndexScanPermitted = (request.LatestScanTimeStamp != null && request.LatestScanTimeStamp?.AddMinutes(request.AdditionalMinutesToLatestScan) > DateTime.Now);
            if(showIndexScanPermitted)
            {
                logger.LogInformation("Scan not necessary");
                downloadShowIndexModel.ScanStatus = ScanStatus.NotPermitted;
                return downloadShowIndexModel;
            }
            try
            {
                logger.LogInformation("Starting download");

                var downloadStopWatch = Stopwatch.StartNew();

                var showIndex = await httpScrapeService.DownloadShowIndex();

                downloadStopWatch.Stop();

                downloadShowIndexModel.ScanStatus = ScanStatus.Complete;
                downloadShowIndexModel.TvMazeShowModel = mapper.Map<IEnumerable<TvMazeShowModel>>(showIndex);
                downloadShowIndexModel.Duration = downloadStopWatch.ElapsedMilliseconds;

                logger.LogInformation($"Download complete with a duration of {downloadStopWatch.ElapsedMilliseconds}ms");
            }
            catch(Exception exception)
            {
                logger.LogError(exception, "Exception during shows download");
                downloadShowIndexModel.ScanStatus = ScanStatus.Failed;
            }

            return downloadShowIndexModel;
        }
    }
}
