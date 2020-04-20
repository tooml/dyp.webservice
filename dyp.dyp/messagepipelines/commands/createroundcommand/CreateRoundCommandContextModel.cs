using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.commands.createroundcommand
{
    public class CreateRoundCommandContextModel : IMessageContext
    {
        public class Player
        {
            public string Id;
            public string First_name;
            public string Last_name;
            public int Matches;
            public bool Enabled;
        }

        public class MatchOptions
        {
            public int Sets;
            public bool Drawn;
        }

        public int Round_count;
        public List<Player> Players;
        public List<string> Walkover_player_ids;
        public int Tables;
        public int Sets;
        public bool Drawn;
        public bool Walkover;
        public MatchOptions MatchOption => new MatchOptions() { Sets = Sets, Drawn = Drawn };
    }
}