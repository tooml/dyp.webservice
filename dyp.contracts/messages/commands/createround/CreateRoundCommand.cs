using dyp.messagehandling;

namespace dyp.contracts.messages.commands.createnewround
{
    public class CreateRoundCommand : Command
    {
        public string TournamentId { get; set; }
    }
}