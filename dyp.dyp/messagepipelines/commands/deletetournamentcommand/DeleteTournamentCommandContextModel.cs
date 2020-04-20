using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.deletetournamentcommand
{
    public class DeleteTournamentCommandContextModel : IMessageContext
    {
        public bool Tournament_existing;
    }
}