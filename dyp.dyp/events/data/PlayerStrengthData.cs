using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class PlayerStrengthData : EventData
    {
        public string Player_id;
        public string Match_id;
        public int Strength_amount;
    }
}
