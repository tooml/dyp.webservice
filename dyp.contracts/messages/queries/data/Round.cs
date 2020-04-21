using System.Collections.Generic;

namespace dyp.contracts.messages.queries.data
{
    public class Round
    {
        public string Id { get; set; }
        public string TournamentId { get; set; }
        public string Name { get; set; }

        public IEnumerable<Match> Matches { get; set; }
    }
}