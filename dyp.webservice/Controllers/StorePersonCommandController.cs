using dyp.contracts.messages.commands.storeperson;
using dyp.dyp.messagepipelines.commands.storepersoncommand;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person")]
    [ApiController]
    public class StorePersonCommandController : ControllerBase
    {
        private readonly ILogger<StorePersonCommandController> _logger;
        private readonly IEventStore _es;

        public StorePersonCommandController(ILogger<StorePersonCommandController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpPost]
        public IActionResult Store_person(StorePersonCommand store_person_command)
        {
            _logger.LogInformation($"store person command, id: { store_person_command.Id }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new StorePersonCommandContextManager(_es);
                var message_processor = new StorePersonCommandProcessor();
                msgpump.Register<StorePersonCommand>(context_manager, message_processor);

                var result = msgpump.Handle(store_person_command) as CommandStatus;
                if (result is Success) return Ok(); else return BadRequest();
            }
        }
    }
}