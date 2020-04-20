using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.queries.personstatsquery
{
    public class PersonStatsQueryContextModel : IMessageContext
    {
        public enum Result
        {
            Won,
            Loose,
            Drawn,
            None
        }

        public class Match
        {
            public string Id;
            public Result Match_result;
        }

        public class Tournament
        {
            public string Id;
            public List<Match> Matches = new List<Match>();
        }

        public List<Tournament> Tournaments = new List<Tournament>();
    }
}