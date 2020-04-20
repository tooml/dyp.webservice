using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class PlayerActivityData : EventData
    {
        public string Player_id;
        public bool Activ;
    }
}