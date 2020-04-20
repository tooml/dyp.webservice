using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.matchresetcommand
{
    public class StoreMatchResetCommandContextModel : IMessageContext
    {
        public string Tournament_id;
        public string Match_id;
        public string[] Player_ids;
    }
}