using dyp.contracts.messages.queries.personstats;
using dyp.dyp.messagepipelines.queries.personstatsquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person/stats")]
    [ApiController]
    public class PersonStatsQueryController : ControllerBase
    {
        private readonly ILogger<PersonStatsQueryController> _logger;
        private readonly IEventStore _es;

        public PersonStatsQueryController(ILogger<PersonStatsQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public PersonStatsQueryResult Load_persons_stats(string personId)
        {
            _logger.LogInformation($"person stats query: { personId }");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new PersonStatsQueryContextManager(_es);
                var message_processor = new PersonStatsQueryProcessor();
                msgpump.Register<PersonStatsQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new PersonStatsQuery() { PersonId = personId }) as PersonStatsQueryResult;
                return result;
            }
        }
    }
}