using System.Collections.Generic;

namespace dyp.contracts.data
{
    public class MatchList
    {
        public IEnumerable<Match> Matches { get; set; }
        public IEnumerable<Player> Walkover { get; set; }
    }
}