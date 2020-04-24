using dyp.contracts.messages.queries.personavtars;
using dyp.dyp.messagepipelines.queries.personavatarsquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person/all/avatar")]
    [ApiController]
    public class PersonAvatarsQueryController : ControllerBase
    {
        private readonly ILogger<PersonAvatarsQueryController> _logger;
        private readonly IEventStore _es;

        public PersonAvatarsQueryController(ILogger<PersonAvatarsQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public PersonAvatarsQueryResult Load_persons_avatars()
        {
            _logger.LogInformation($"person avatars query");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new PersonAvtarsQueryContextManager(_es);
                var message_processor = new PersonAvtarsQueryProcessor();
                msgpump.Register<PersonAvatarsQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new PersonAvatarsQuery()) as PersonAvatarsQueryResult;
                return result;
            }
        }
    }
}