using dyp.contracts.messages.queries.data;
using dyp.messagehandling;

namespace dyp.contracts.messages.queries.tournament
{
    public class TournamentQueryResult : QueryResult
    {
        public Tournament Tournament { get; set; }
    }
}