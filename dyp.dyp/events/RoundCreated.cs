using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class RoundCreated : Event
    {
        public RoundCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}