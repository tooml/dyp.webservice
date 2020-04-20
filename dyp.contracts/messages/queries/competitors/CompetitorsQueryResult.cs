using dyp.messagehandling;

namespace dyp.contracts.messages.queries.competitors
{
    public class CompetitorsQueryResult : QueryResult
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