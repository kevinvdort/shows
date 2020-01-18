using MediatR;
using RTL.TvMaze.Infrastructure.Repositories;
using RTL.TvMaze.Infrastructure.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using RTL.TvMaze.Domain.TvMaze.Comparers;
using System.Collections.Generic;
using RTL.TvMaze.Domain.TvMaze.Models;

namespace RTL.TvMaze.Domain.TvMaze.Commands
{
    public class InsertOrUpdateShowAndCastCommandHandler : IRequestHandler<InsertOrUpdateShowAndCastCommand, bool>
    {
        private readonly ITvMazeShowRepository tvMazeShowRepository;
        private readonly ITvMazeShowIndexRepository tvMazeShowIndexRepository;
        private readonly ITvMazePersonRepository tvMazePersonRepository;
        private readonly ITvMazeShowCastRepository tvMazeShowCastRepository;
        private readonly ITvMazeCastModelEqualityComparer tvMazeCastModelEqualityComparer;
        private readonly ILogger<InsertOrUpdateShowAndCastCommandHandler> logger;

        public InsertOrUpdateShowAndCastCommandHandler(ITvMazeShowRepository tvMazeShowRepository,
                                                       ITvMazeShowIndexRepository tvMazeShowIndexRepository,
                                                       ITvMazePersonRepository tvMazePersonRepository,
                                                       ITvMazeShowCastRepository tvMazeShowCastRepository,
                                                       ITvMazeCastModelEqualityComparer tvMazeCastModelEqualityComparer,
                                                       ILogger<InsertOrUpdateShowAndCastCommandHandler> logger)
        {
            this.tvMazeShowRepository = tvMazeShowRepository;
            this.tvMazeShowIndexRepository = tvMazeShowIndexRepository;
            this.tvMazePersonRepository = tvMazePersonRepository;
            this.tvMazeShowCastRepository = tvMazeShowCastRepository;
            this.tvMazeCastModelEqualityComparer = tvMazeCastModelEqualityComparer;
            this.logger = logger;
        }

        public async Task<bool> Handle(InsertOrUpdateShowAndCastCommand request,
                                       CancellationToken cancellationToken)
        {
            if(request is null)
            {
                return false;
            }

            try
            {
                logger.LogInformation("Inserting data into database");

                var showIndex = new TvMazeShowIndex
                {
                    Duration = request.Duration,
                    Timestamp = DateTime.Now,
                    InProgress = true
                };
                await tvMazeShowIndexRepository.AddAsync(showIndex);
                await tvMazeShowIndexRepository.SaveChangesAsync();

                int showCreatedCounter = 0;

                foreach (var showModel in request.TvMazeShowModelCollection)
                {
                    var show = await tvMazeShowRepository.GetByTvMazeIdAsync(showModel.Id);

                    if (show == null)
                    {
                        show = new TvMazeShow
                        {
                            TvMazeId = showModel.Id,
                            Name = showModel.Name
                        };

                        await tvMazeShowRepository.AddAsync(show);

                        showCreatedCounter++;
                    }

                    await ProcessPersonAndCastInformation(showModel.Id, request.TvMazeCastModelCollection);

                    // RTLDbContext.SaveChangesAsync() -> All the entities up to this point will be stored into the db.
                    await tvMazeShowRepository.SaveChangesAsync();
                }

                logger.LogInformation($"Records created: {showCreatedCounter}");

                showIndex.InProgress = false;
                showIndex.RecordsCreated = showCreatedCounter;

                await tvMazeShowIndexRepository.SaveChangesAsync();

                logger.LogInformation("Done inserting");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed when inserting data into database");
                return false;
            }

            return true;
        }

        private async Task ProcessPersonAndCastInformation(int showId, IEnumerable<TvMazeCastModel> cast)
        {
            var tvMazePersonModelCollection = cast.Where(x => x.ShowId == showId)
                                                  .Distinct(tvMazeCastModelEqualityComparer)
                                                  .Select(x => x.Person);

            var showCast = await tvMazeShowCastRepository.GetByShowIdAsync(showId);

            foreach (var people in tvMazePersonModelCollection)
            {
                var person = await tvMazePersonRepository.GetByTvMazeIdAsync(people.Id);

                if (person == null)
                {
                    person = new TvMazePerson
                    {
                        TvMazeId = people.Id,
                        Name = people.Name,
                        Birthday = people.Birthday
                    };

                    await tvMazePersonRepository.AddAsync(person);
                }

                var personInCast = showCast.FirstOrDefault(x => x.TvMazePersonId == people.Id);

                if (personInCast == null)
                {
                    personInCast = new TvMazeShowCast
                    {
                        TvMazePersonId = person.TvMazeId,
                        TvMazeShowId = showId
                    };
                    await tvMazeShowCastRepository.AddAsync(personInCast);
                }
            }
        }
    }
}
