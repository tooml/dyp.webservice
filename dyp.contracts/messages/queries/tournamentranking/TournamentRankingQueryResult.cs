using dyp.contracts.messages.queries.data;
using dyp.messagehandling;

namespace dyp.contracts.messages.queries.tournamentranking
{
    public class TournamentRankingQueryResult : QueryResult
    {
        public RankingRow[] Ranking { get; set; }
    }
}