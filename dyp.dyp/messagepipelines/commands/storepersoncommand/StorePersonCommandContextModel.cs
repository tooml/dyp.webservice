using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.storepersoncommand
{
    public class StorePersonCommandContextModel : IMessageContext
    {
        public bool Person_existing;
    }
}