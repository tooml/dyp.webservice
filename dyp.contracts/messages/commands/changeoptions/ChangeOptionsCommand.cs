using dyp.messagehandling;

namespace dyp.contracts.messages.commands.changeoptions
{
    public class ChangeOptionsCommand : Command
    {
        public string TournamentId;
        public int Tables;
        public int Sets;
        public int Points;
        public int PointsDrawn;
        public bool Drawn;
        public bool Walkover;
    }
}