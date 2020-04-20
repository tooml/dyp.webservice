using dyp.provider.eventstore;
using System.Collections.Generic;

namespace dyp.dyp.events.data
{
    public enum SetResult
    {
        Home,
        Away,
        Drawn,
        None
    }

    public class MatchResultData : EventData
    {
        public string Match_id;
        public IEnumerable<SetResult> Results;
    }
}
