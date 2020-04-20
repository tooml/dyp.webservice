using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class MatchResetData : EventData
    {
        public string Tournament_id;
        public string Match_id;
    }
}
