using dyp.contracts.messages.queries.tournamentstock;
using dyp.dyp.messagepipelines.queries.tournamentstockquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/all")]
    [ApiController]
    public class TournamentStockQueryController : ControllerBase
    {
        private readonly ILogger<TournamentStockQueryController> _logger;
        private readonly IEventStore _es;

        public TournamentStockQueryController(ILogger<TournamentStockQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public TournamentStockQueryResult Get_tournaments_infos()
        {
            _logger.LogInformation("tournaments stock query");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new TournamentStockQueryContextManager(_es);
                var message_processor = new TournamentStockQueryProcessor();
                msgpump.Register<TournamentStockQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new TournamentStockQuery()) as TournamentStockQueryResult;
                return result;
            }
        }
    }
}