using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class Player
    {
        public string Id;
        public string First_name;
        public string Last_name;
    }

    public class PlayerData : EventData
    {
        public Player Player;
    }
}