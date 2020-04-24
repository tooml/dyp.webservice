using dyp.messagehandling;

namespace dyp.contracts.messages.queries.tournamentplayers
{
    public class TournamentCompetitorsQueryResult : QueryResult
    {
        public class Competitor
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Compete { get; set; }
        }

        public Competitor[] Competitors { get; set; }
    }
}