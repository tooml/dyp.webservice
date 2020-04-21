using dyp.messagehandling;

namespace dyp.contracts.messages.commands.changeoptions
{
    public class ChangeOptionsCommand : Command
    {
        public string TournamentId { get; set; }
        public int Tables { get; set; }
        public int Sets { get; set; }
        public int Points { get; set; }
        public int PointsDrawn { get; set; }
        public bool Drawn { get; set; }
        public bool Walkover { get; set; }
    }
}