using dyp.messagehandling;

namespace dyp.contracts.messages.queries.tournamentplayers
{
    public class TournamentCompetitorsQueryResult : QueryResult
    {
        public class Competitor
        {
            public string Id;
            public string Name;
            public bool Compete;
        }

        public Competitor[] Competitors { get; set; }
    }
}