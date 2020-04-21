using dyp.messagehandling;

namespace dyp.contracts.messages.queries.personstats
{
    public class PersonStatsQueryResult : QueryResult
    {
        public int Tournaments { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Loose { get; set; }
        public int Drawn { get; set; }
    }
}