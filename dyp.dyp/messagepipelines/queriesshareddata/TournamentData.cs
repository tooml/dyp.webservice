using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queriesshareddata
{
    public class TournamentData
    {
        public enum SetResult
        {
            Home,
            Away,
            Drawn,
            None
        }

        public class Player
        {
            public string Id;
            public string First_name;
            public string Last_name;
        }

        public class Team
        {
            public Player Player_one;
            public Player Player_two;
        }

        public class Match
        {
            public string Id;
            public Team Home;
            public Team Away;
            public int Table;
            public int Sets;
            public bool Drawn;
            public List<SetResult> Results;
        }

        public class Round
        {
            public string Id;
            public int Count;
            public List<Match> Matches = new List<Match>();
        }

        public IEnumerable<SetResult> Create_default_sets(int count)
        {
            return Enumerable.Range(1, count).Select(set_number => SetResult.None);
        }
    }
}