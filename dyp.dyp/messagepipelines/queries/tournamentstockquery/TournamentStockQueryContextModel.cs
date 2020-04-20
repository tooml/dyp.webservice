using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.queries.tournamentstockquery
{
    public class TournamentStockQueryContextModel : IMessageContext
    {
        public class Tournament
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Created { get; set; }
        }

        public List<Tournament> Tournaments { get; set; }
    }
}