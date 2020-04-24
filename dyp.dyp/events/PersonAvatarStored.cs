using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonAvatarStored : Event
    {
        public PersonAvatarStored(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}