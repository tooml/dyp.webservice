using dyp.provider.eventstore;

namespace dyp.dyp.events.context
{
    public class TournamentContext : EventContext
    {
        public TournamentContext(string id, string name) : base(id, name) { }
    }
}