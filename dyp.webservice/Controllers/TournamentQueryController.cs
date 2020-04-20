using dyp.contracts.messages.queries.tournament;
using dyp.dyp.messagepipelines.queries.tournamentquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament")]
    [ApiController]
    public class TournamentQueryController : ControllerBase
    {
        private readonly ILogger<TournamentQueryController> _logger;
        private readonly IEventStore _es;

        public TournamentQueryController(ILogger<TournamentQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public TournamentQueryResult Load_tournament(string tournamentId)
        {
            _logger.LogInformation($"tournament query, tournament: { tournamentId }");

            using (var msgpump = new MessagePump())
            {
                var context_manager = new TournamentQueryContextManager(_es);
                var message_processor = new TournamentQueryProcessor();
                msgpump.Register<TournamentQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new TournamentQuery() { TournamentId = tournamentId }) as TournamentQueryResult;
                return result;
            }
        }
    }
}