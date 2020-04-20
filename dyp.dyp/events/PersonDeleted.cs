using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonDeleted : Event
    {
        public PersonDeleted(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}