using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class MatchCreated : Event
    {
        public MatchCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}