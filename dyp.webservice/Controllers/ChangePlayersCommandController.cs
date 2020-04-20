using dyp.contracts.messages.commands.addplayer;
using dyp.dyp.messagepipelines.commands.changeplayerscommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/players/change")]
    [ApiController]
    public class ChangePlayersCommandController : ControllerBase
    {
        private readonly ILogger<ChangePlayersCommandController> _logger;
        private readonly IEventStore _es;

        public ChangePlayersCommandController(ILogger<ChangePlayersCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Change_players(ChangePlayersCommand change_players_command)
        {
            _logger.LogInformation($"change players command: { change_players_command.TournamentId }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new ChangePlayersCommandContextManager(_es);
                var message_processor = new ChangePlayersCommandProcessor();
                msgpump.Register<ChangePlayersCommand>(context_manager, message_processor);

                var result = msgpump.Handle(change_players_command) as CommandStatus;

                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}