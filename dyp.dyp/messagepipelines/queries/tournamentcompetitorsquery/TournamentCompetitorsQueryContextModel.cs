using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.queries.tournamentplayersquery
{
    public class TournamentCompetitorsQueryContextModel : IMessageContext
    {
        public class Competitor
        {
            public string Id;
            public string First_name;
            public string Last_name;
            public bool Enabled;
        }

        public List<Competitor> Competitors = new List<Competitor>();
    }
}