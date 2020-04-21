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
        public string Id { get; set; }

        public Team Home { get; set; }
        public Team Away { get; set; }

        public int Table { get; set; }
        public int Sets { get; set; }
        public bool Drawn { get; set; }

        public IEnumerable<SetResult> SetResults { get; set; }
    }
}