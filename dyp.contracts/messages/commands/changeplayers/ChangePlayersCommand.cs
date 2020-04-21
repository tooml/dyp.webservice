using dyp.messagehandling;

namespace dyp.contracts.messages.commands.addplayer
{
    public class ChangePlayersCommand : Command
    {
        public string TournamentId { get; set; }
        public string[] PlayerIds { get; set; }
    }
}