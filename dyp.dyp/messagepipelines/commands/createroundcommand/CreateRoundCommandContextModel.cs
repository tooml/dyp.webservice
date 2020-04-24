using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;
using System.Linq;

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
            public int Strength;
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
        public bool Fair_lots;
        public MatchOptions MatchOption => new MatchOptions() { Sets = Sets, Drawn = Drawn };

        public List<(string Player_one, string Player_two)> Previous_teams = new List<(string Player_one, string Player_two)>();

        public Player Get_player(string player_id) => Players.First(player => player.Id.Equals(player_id));
    }
}