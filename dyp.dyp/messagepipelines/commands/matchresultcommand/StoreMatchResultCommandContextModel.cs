using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.matchresultcommand
{
    public class StoreMatchResultCommandContextModel : IMessageContext
    {
        public string Tournament_id;
        public string[] Home_player_ids;
        public string[] Away_player_ids;
    }
}