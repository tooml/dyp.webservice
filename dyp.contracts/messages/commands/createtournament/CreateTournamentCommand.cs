using dyp.messagehandling;

namespace dyp.contracts.messages.commands.createtournament
{
    public class CreateTournamentCommand : Command
    {
        public string Name { get; set; }
        public int Tables { get; set; }
        public int Sets { get; set; }
        public int Points { get; set; }
        public int PointsDrawn { get; set; }
        public bool Drawn { get; set; }
        public bool FairLots { get; set; }
        public string[] CompetitorsIds { get; set; }
    }
}