using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class MatchReseted : Event
    {
        public MatchReseted(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}