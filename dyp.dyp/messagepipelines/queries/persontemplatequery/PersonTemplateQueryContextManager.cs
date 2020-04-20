using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.queries.persontemplatequery
{
    public class PersonTemplateQueryContextManager : IMessageContextManager
    {
        public IMessageContext Load(IMessage input) => new PersonTemplateQueryContextModel();

        public void Update(IEnumerable<Event> events) { }
    }
}