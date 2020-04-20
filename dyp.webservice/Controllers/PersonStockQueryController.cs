using dyp.contracts.messages.queries.personstock;
using dyp.dyp.messagepipelines.queries.personsstockquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person/all")]
    [ApiController]
    public class PersonStockQueryController : ControllerBase
    {
        private readonly ILogger<PersonStockQueryController> _logger;
        private readonly IEventStore _es;

        public PersonStockQueryController(ILogger<PersonStockQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public PersonStockQueryResult Load_persons()
        {
            _logger.LogInformation("person stock query");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new PersonStockQueryContextManager(_es);
                var message_processor = new PersonStockQueryProcessor();
                msgpump.Register<PersonStockQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new PersonStockQuery()) as PersonStockQueryResult;
                return result;
            }
        }
    }
}