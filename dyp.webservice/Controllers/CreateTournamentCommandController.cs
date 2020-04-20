using dyp.adapter;
using dyp.contracts.messages.commands.createtournament;
using dyp.dyp.messagepipelines.commands.createtournamentcommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament")]
    [ApiController]
    public class CreateTournamentCommandController : ControllerBase
    {
        private readonly ILogger<CreateTournamentCommandController> _logger;
        private readonly IEventStore _es;

        public CreateTournamentCommandController(ILogger<CreateTournamentCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Create_tournament(CreateTournamentCommand create_tournament_command)
        {
            _logger.LogInformation($"create tournament command: { create_tournament_command.Name }");

            using (var msgpump = new MessagePump(_es))
            {
                var id_provider = new IdProvider();
                var date_provider = new DateProvider();

                var context_manager = new CreateTournamentCommandContextManager(_es);
                var message_processor = new CreateTournamentCommandProcessor(id_provider, date_provider);
                msgpump.Register<CreateTournamentCommand>(context_manager, message_processor);

                var result = msgpump.Handle(create_tournament_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}