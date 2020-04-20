using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonMatchStatsCreated : Event
    {
        public PersonMatchStatsCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}