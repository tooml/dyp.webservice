using dyp.contracts.messages.queries.competitors;
using dyp.dyp.messagepipelines.queries.competitorsquery;
using dyp.messagehandling;
using dyp.provider.eventstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/competitors")]
    [ApiController]
    public class CompetitorsQueryController : ControllerBase
    {
        private readonly ILogger<CompetitorsQueryController> _logger;
        private readonly IEventStore _es;

        public CompetitorsQueryController(ILogger<CompetitorsQueryController> logger, IEventStore es)
        {
            _logger = logger;
            _es = es;
        }

        [HttpGet]
        public CompetitorsQueryResult Get_competitors()
        {
            _logger.LogInformation("competitors query");

            using (var msgpump = new MessagePump(_es))
            {
                var context_manager = new CompetitorsQueryContextManager(_es);
                var message_processor = new CompetitorsQueryProcessor();
                msgpump.Register<CompetitorsQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new CompetitorsQuery()) as CompetitorsQueryResult;
                return result;
            }
        }
    }
}