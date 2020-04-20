using dyp.messagehandling;

namespace dyp.contracts.messages.queries.personstats
{
    public class PersonStatsQueryResult : QueryResult
    {
        public int Tournaments;
        public int Matches;
        public int Wins;
        public int Loose;
        public int Drawn;
    }
}