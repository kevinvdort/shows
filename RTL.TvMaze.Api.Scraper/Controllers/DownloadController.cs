using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RTL.TvMaze.Domain.TvMaze.Queries;
using RTL.TvMaze.Domain.TvMaze.Commands;
using System.Linq;
using RTL.TvMaze.Domain.TvMaze.Enumerations;
using RTL.TvMaze.Api.Scraper.Models;
using Microsoft.Extensions.Options;
using RTL.TvMaze.Api.Scraper.Configurations;

namespace RTL.TvMaze.Api.Scraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IOptions<DownloadSettings> downloadSettings;
        private readonly IMediator mediator;

        public DownloadController(IOptions<DownloadSettings> downloadSettings,
                                  IMediator mediator)
        {
            this.downloadSettings = downloadSettings;
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("shows")]
        [ProducesResponseType(200, Type = typeof(DownloadModel))]
        public async Task<ActionResult> Shows()
        {
            var downloadModel = new DownloadModel();

            var showIndex = await mediator.Send(new GetLatestShowIndexScanQuery());

            var requestShowIndexQuery = new DownloadShowIndexQuery
            {
                AdditionalMinutesToLatestScan = downloadSettings.Value.AdditionalMinutesToLatestScan,
                LatestScanTimeStamp = showIndex?.TimeStamp
            };
            var requestShowIndexModel = await mediator.Send(requestShowIndexQuery);

            if (requestShowIndexModel.ScanStatus == ScanStatus.Complete)
            {
                var downloadCastFromShowsQuery = new DownloadCastFromShowsQuery
                {
                    Shows = requestShowIndexModel.TvMazeShowModel.Select(t => t.Id)
                };
                var downloadCastFromShowsQueryResult = await mediator.Send(downloadCastFromShowsQuery);

                var insertOrUpdateShowAndCastCommand = new InsertOrUpdateShowAndCastCommand
                {
                    Duration = requestShowIndexModel.Duration,
                    TvMazeShowModelCollection = requestShowIndexModel.TvMazeShowModel,
                    TvMazeCastModelCollection = downloadCastFromShowsQueryResult.TvMazePeopleModelCollection
                };
                var commandInsertResult = await mediator.Send(insertOrUpdateShowAndCastCommand);

                downloadModel.Result = true;
                downloadModel.Message = "Show scan is complete.";
            }
            else if (requestShowIndexModel.ScanStatus == ScanStatus.NotPermitted)
            {
                downloadModel.Result = false;
                downloadModel.Message = "Show scan is not permitted at this moment.";
            }
            else
            {
                downloadModel.Result = false;
                downloadModel.Message = "Show scan has failed.";
            }

            return new JsonResult(downloadModel);
        }
    }
}
