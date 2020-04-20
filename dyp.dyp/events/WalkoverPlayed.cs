using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class WalkoverPlayed : Event
    {
        public WalkoverPlayed(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}