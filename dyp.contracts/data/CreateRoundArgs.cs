using System.Collections.Generic;

namespace dyp.contracts.data
{
    public class CreateRoundArgs
    {
        public IEnumerable<Player> Players;
        public int Tables;
        public bool Fair_lots;
        public List<(string Player_one, string Player_two)> Previous_teams;
    }
}