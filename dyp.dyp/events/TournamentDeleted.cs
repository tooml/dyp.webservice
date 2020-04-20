using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class TournamentDeleted : Event
    {
        public TournamentDeleted(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}