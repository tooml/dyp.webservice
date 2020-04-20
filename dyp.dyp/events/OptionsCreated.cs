using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class OptionsCreated : Event
    {
        public OptionsCreated(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}