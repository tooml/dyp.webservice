using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class TournamentCreated : Event
    {
        public TournamentCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}