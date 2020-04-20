using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;
using dyp.dyp.messagepipelines.queriesshareddata;

namespace dyp.dyp.messagepipelines.queries.tournamentroundquery
{
    public class TournamentRoundQueryContextModel : TournamentData, IMessageContext
    {
        public string Tournament_Id;
        public string Id;
        public int Count;
        public List<Match> Matches = new List<Match>();
    }
}