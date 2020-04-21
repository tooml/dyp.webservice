using dyp.messagehandling;

namespace dyp.contracts.messages.commands.deletetournamentcommand
{
    public class DeleteTournamentCommand : Command
    {
        public string Id { get; set; }
    }
}