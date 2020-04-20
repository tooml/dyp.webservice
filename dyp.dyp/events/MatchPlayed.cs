using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class MatchPlayed : Event
    {
        public MatchPlayed(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}