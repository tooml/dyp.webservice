using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class MatchData : EventData
    {
        public class Team
        {
            public Player Player_one;
            public Player Player_two;
        }

        public string Id;
        public string Round_id;

        public Team Home;
        public Team Away;

        public int Table;
        public int Sets;
        public bool Drawn;
    }
}