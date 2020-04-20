using dyp.contracts.messages.queries.tournamentranking;
using dyp.dyp.messagepipelines.queries.tournamentrankingquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/ranking")]
    [ApiController]
    public class TournamentRankingQueryController : ControllerBase
    {
        private readonly ILogger<TournamentRankingQueryController> _logger;
        private readonly IEventStore _es;

        public TournamentRankingQueryController(ILogger<TournamentRankingQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public TournamentRankingQueryResult Get_tournament_round(string tournamentId)
        {
            _logger.LogInformation($"tournament ranking query, tournament: {tournamentId}");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new TournamentRankQueryContextManager(_es);
                var message_processor = new TournamentRankingQueryProcessor();
                msgpump.Register<TournamentRankingQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new TournamentRankingQuery() { TournamentId = tournamentId }) as TournamentRankingQueryResult;
                return result;
            }
        }
    }
}