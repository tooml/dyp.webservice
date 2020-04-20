using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonStored : Event
    {
        public PersonStored(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}