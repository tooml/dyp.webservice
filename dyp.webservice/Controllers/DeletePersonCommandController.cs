using dyp.contracts.messages.commands.deleteperson;
using dyp.dyp.messagepipelines.commands.deletepersoncommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person/delete")]
    [ApiController]
    public class DeletePersonCommandController : ControllerBase
    {
        private readonly ILogger<DeletePersonCommandController> _logger;
        private readonly IEventStore _es;

        public DeletePersonCommandController(ILogger<DeletePersonCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Delete_person(DeletePersonCommand delete_person_command)
        {
            _logger.LogInformation($"delete person command, id: { delete_person_command.Id }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new DeletePersonCommandContextManager(_es);
                var message_processor = new DeletePersonCommandProcessor();
                msgpump.Register<DeletePersonCommand>(context_manager, message_processor);

                var result = msgpump.Handle(delete_person_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}