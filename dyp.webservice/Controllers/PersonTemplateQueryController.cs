using dyp.adapter;
using dyp.contracts.messages.queries.persontemplate;
using dyp.dyp.messagepipelines.queries.persontemplatequery;
using dyp.messagehandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    [Route("/api/v1/person/template")]
    [ApiController]
    public class PersonTemplateQueryController : ControllerBase
    {
        private readonly ILogger<PersonTemplateQueryController> _logger;

        public PersonTemplateQueryController(ILogger<PersonTemplateQueryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public PersonTemplateQueryResult Get_person_template()
        {
            _logger.LogInformation("person template query");

            using (var msgpump = new MessagePump())
            {
                var id_provider = new IdProvider();

                var context_manager = new PersonTemplateQueryContextManager();
                var message_processor = new PersonTemplateQueryProcessor(id_provider);
                msgpump.Register<PersonTemplateQuery>(context_manager, message_processor);

                var result = msgpump.Handle(new PersonTemplateQuery()) as PersonTemplateQueryResult;
                return result;
            }
        }
    }
}