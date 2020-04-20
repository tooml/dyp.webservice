using System;
using dyp.adapter;
using dyp.contracts.messages.commands.createnewround;
using dyp.dyp.messagepipelines.commands.createroundcommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/tournament/round")]
    [ApiController]
    public class CreateRoundCommandController : ControllerBase
    {
        private readonly ILogger<CreateRoundCommandController> _logger;
        private readonly IEventStore _es;

        public CreateRoundCommandController(ILogger<CreateRoundCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Create_round(CreateRoundCommand create_round_command)
        {
            _logger.LogInformation($"create round command: { create_round_command.TournamentId }");

            using (var msgpump = new MessagePump(_es))
            {
                var id_provider = new IdProvider();

                var context_manager = new CreateRoundCommandContextManager(_es);
                var message_processor = new CreateRoundCommandProcessor(id_provider);
                msgpump.Register<CreateRoundCommand>(context_manager, message_processor);

                var result = msgpump.Handle(create_round_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}