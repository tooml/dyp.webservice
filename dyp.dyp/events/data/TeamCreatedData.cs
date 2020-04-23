using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class TeamCreatedData : EventData
    {
        public string Player_one_id;
        public string Player_two_id;
    }
}