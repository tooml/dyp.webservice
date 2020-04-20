using System.Collections.Generic;

namespace dyp.contracts.data
{
    public class Round
    {
        public string Name { get; set; }
        public IEnumerable<Match> Matches { get; set; }
        public IEnumerable<Player> Walkover { get; set; }
    }
}