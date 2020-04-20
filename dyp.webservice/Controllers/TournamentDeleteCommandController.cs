using dyp.contracts.messages.commands.deletetournamentcommand;
using dyp.dyp.messagepipelines.commands.deletetournamentcommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/delete")]
    [ApiController]
    public class TournamentDeleteCommandController : ControllerBase
    {
        private readonly ILogger<TournamentDeleteCommandController> _logger;
        private readonly IEventStore _es;

        public TournamentDeleteCommandController(ILogger<TournamentDeleteCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Delete_tournament(DeleteTournamentCommand delete_tournament_command)
        {
            _logger.LogInformation($"delete tournament command, id: { delete_tournament_command.Id }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new DeleteTournamentCommandContextManager(_es);
                var message_processor = new DeleteTournamentCommandProcessor();
                msgpump.Register<DeleteTournamentCommand>(context_manager, message_processor);

                var result = msgpump.Handle(delete_tournament_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}