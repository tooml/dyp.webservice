using dyp.messagehandling;

namespace dyp.contracts.messages.commands.addplayer
{
    public class ChangePlayersCommand : Command
    {
        public string TournamentId;
        public string[] PlayerIds;
    }
}