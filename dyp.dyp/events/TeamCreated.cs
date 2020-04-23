using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class TeamCreated : Event
    {
        public TeamCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}