using dyp.provider.eventstore;

namespace dyp.dyp.events.context
{
    public class PersonsContext : EventContext
    {
        public PersonsContext(string id, string name) : base(id, name) { }
    }
}