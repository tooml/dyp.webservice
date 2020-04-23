using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PlayerStrengthChanged : Event
    {
        public PlayerStrengthChanged(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}