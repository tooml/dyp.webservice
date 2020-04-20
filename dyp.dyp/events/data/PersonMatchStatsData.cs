using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class PersonMatchStatsData : EventData
    {
        public enum Result
        {
            Won,
            Loose,
            Drawn,
            None
        }

        public string Person_id;
        public string Tournament_id;
        public string Match_id;

        public Result Match_result;
    }
}