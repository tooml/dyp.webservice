using System.Collections.Generic;

namespace dyp.contracts.messages.queries.data
{
    public class Round
    {
        public string Id;
        public string TournamentId;
        public string Name;

        public IEnumerable<Match> Matches;
    }
}