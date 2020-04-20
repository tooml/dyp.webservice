using System;
using dyp.contracts.messages.queries.tournamentround;
using dyp.dyp.messagepipelines.queries.tournamentroundquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/round/last")]
    [ApiController]
    public class TournamentRoundQueryController : ControllerBase
    {
        private readonly ILogger<TournamentRoundQueryController> _logger;
        private readonly IEventStore _es;

        public TournamentRoundQueryController(ILogger<TournamentRoundQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public TournamentRoundQueryResult Get_tournament_round(string tournamentId)
        {
            _logger.LogInformation($"tournament round query, tournament: {tournamentId}");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new TournamentRoundQueryContextManager(_es);
                var message_processor = new TournamentRoundQueryProcessor();
                msgpump.Register<TournamentRoundQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new TournamentRoundQuery() { TournamentId = tournamentId }) as TournamentRoundQueryResult;
                return result;
            }
        }
    }
}