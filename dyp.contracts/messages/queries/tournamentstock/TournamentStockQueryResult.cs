using dyp.contracts.messages.queries.data;
using dyp.messagehandling;
using System.Collections.Generic;

namespace dyp.contracts.messages.queries.tournamentstock
{
    public class TournamentStockQueryResult : QueryResult
    {
        public List<Tournament> Tournaments { get; set; }
    }
}