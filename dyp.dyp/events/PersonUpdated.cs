using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonUpdated : Event
    {
        public PersonUpdated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}