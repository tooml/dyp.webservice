using dyp.contracts.messages.queries.data;
using dyp.messagehandling;

namespace dyp.contracts.messages.queries.tournamentround
{
    public class TournamentRoundQueryResult : QueryResult
    {
        public Round Round { get; set; }
    }
}