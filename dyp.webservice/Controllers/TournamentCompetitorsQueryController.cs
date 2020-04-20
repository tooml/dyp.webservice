using dyp.contracts.messages.queries.tournamentplayers;
using dyp.dyp.messagepipelines.queries.tournamentplayersquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/competitors")]
    [ApiController]
    public class TournamentCompetitorsQueryController : ControllerBase
    {
        private readonly ILogger<TournamentCompetitorsQueryController> _logger;
        private readonly IEventStore _es;

        public TournamentCompetitorsQueryController(ILogger<TournamentCompetitorsQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public TournamentCompetitorsQueryResult Load_tournament_competitors(string tournamentId)
        {
            _logger.LogInformation($"tournament competitors query, tournament: { tournamentId }");

            using (var msgpump = new MessagePump())
            {
                var context_manager = new TournamentCompetitorsQueryContextManager(_es);
                var message_processor = new TournamentCompetitorsQueryContextProcessor();
                msgpump.Register<TournamentCompetitorsQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new TournamentCompetitorsQuery() { TournamentId = tournamentId }) as TournamentCompetitorsQueryResult;
                return result;
            }
        }
    }
}