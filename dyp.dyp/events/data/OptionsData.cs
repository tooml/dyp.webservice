using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class OptionsData : EventData
    {
        public int Tables;
        public int Sets;
        public int Points;
        public int Points_drawn;
        public bool Drawn;
        public bool Walkover;
    }
}