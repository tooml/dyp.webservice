using dyp.contracts.messages.commands.matchresult;
using dyp.dyp.messagepipelines.commands.matchresultcommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/match/result")]
    [ApiController]
    public class MatchResultNotificationController : ControllerBase
    {
        private readonly ILogger<MatchResultNotificationController> _logger;
        private readonly IEventStore _es;

        public MatchResultNotificationController(ILogger<MatchResultNotificationController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Store_match_result(MatchResultNotificationCommand match_result_command)
        {
            var results = string.Join(" ", match_result_command.Results);
            _logger.LogInformation($"match result command, match: {match_result_command.MatchId}, result notification: { results }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new StoreMatchResultCommandContextManager(_es);
                var message_processor = new StoreMatchResultCommandProcessor();
                msgpump.Register<MatchResultNotificationCommand>(context_manager, message_processor);

                var result = msgpump.Handle(match_result_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}