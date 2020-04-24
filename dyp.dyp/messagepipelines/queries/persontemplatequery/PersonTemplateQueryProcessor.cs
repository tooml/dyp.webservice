using dyp.adapter;
using dyp.contracts.messages.queries.persontemplate;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;

namespace dyp.dyp.messagepipelines.queries.persontemplatequery
{
    public class PersonTemplateQueryProcessor : IMessageProcessor
    {
        private readonly IIdProvider _id_provider;

        public PersonTemplateQueryProcessor(IIdProvider id_provider)
        {
            _id_provider = id_provider;
        }

        public Output Process(IMessage input, IMessageContext model)
        {
            return new QueryOutput(new PersonTemplateQueryResult()
            {
                Id = _id_provider.Get_new_id().ToString(),
                FirstName = string.Empty,
                LastName = string.Empty
            });
        }
    }
}