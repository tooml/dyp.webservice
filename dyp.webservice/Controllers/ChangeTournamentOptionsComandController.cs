using dyp.contracts.messages.commands.changeoptions;
using dyp.dyp.messagepipelines.commands.changeoptionscommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/options/change")]
    [ApiController]
    public class ChangeTournamentOptionsComandController : ControllerBase
    {
        private readonly ILogger<ChangeTournamentOptionsComandController> _logger;
        private readonly IEventStore _es;

        public ChangeTournamentOptionsComandController(ILogger<ChangeTournamentOptionsComandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Change_options(ChangeOptionsCommand change_options_command)
        {
            _logger.LogInformation($"change options command: { change_options_command.TournamentId }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new ChangeOptionsCommandContextManager();
                var message_processor = new ChangeOptionsCommandProcessor();
                msgpump.Register<ChangeOptionsCommand>(context_manager, message_processor);

                var result = msgpump.Handle(change_options_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}