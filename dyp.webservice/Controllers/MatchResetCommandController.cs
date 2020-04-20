using dyp.contracts.messages.commands.matchreset;
using dyp.dyp.messagepipelines.commands.matchresetcommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/match/reset")]
    [ApiController]
    public class MatchResetCommandController : ControllerBase
    {
        private readonly ILogger<MatchResetCommandController> _logger;
        private readonly IEventStore _es;

        public MatchResetCommandController(ILogger<MatchResetCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Reset_match_result(MatchResetCommand match_reset_command)
        {
            _logger.LogInformation($"reset match command: {match_reset_command.MatchId}");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new StoreMatchResetCommandContextManager(_es);
                var message_processor = new StoreMatchResetCommandContextProcessor();
                msgpump.Register<MatchResetCommand>(context_manager, message_processor);

                var result = msgpump.Handle(match_reset_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}