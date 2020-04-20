using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.deletepersoncommand
{
    public class DeletePersonCommandContextModel : IMessageContext
    {
        public bool Person_existing;
    }
}