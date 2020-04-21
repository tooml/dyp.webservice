using dyp.messagehandling;

namespace dyp.contracts.messages.queries.competitors
{
    public class CompetitorsQueryResult : QueryResult
    {
        public class Competitor
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Compete { get; set; }
            public string Image { get; set; }
        }

        public Competitor[] Competitors { get; set; }
    }
}