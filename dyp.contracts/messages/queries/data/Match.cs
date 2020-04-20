using System.Collections.Generic;

namespace dyp.contracts.messages.queries.data
{
    public enum SetResult
    {
        Home = 0,
        Away = 1,
        Drawn = 2,
        None = 3,
    }

    public class Match
    {
        public string Id;

        public Team Home;
        public Team Away;

        public int Table;
        public int Sets;
        public bool Drawn;

        public IEnumerable<SetResult> SetResults;
    }
}