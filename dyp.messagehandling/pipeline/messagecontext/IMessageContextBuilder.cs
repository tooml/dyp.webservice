using dyp.provider.eventstore;
using System.Collections.Generic;

namespace dyp.messagehandling.pipeline.messagecontext
{
    public interface IMessageContextBuilder
    {
        void Update(IEnumerable<Event> events);
    }
}