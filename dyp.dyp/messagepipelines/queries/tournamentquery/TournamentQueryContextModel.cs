using dyp.dyp.messagepipelines.queriesshareddata;
using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentquery
{
    public class TournamentQueryContextModel : TournamentData, IMessageContext
    {
        public string Id;
        public string Name;
        public string Created;

        public int Tables;
        public int Sets;
        public int Points;
        public int Points_drawn;
        public bool Drawn;
        public bool Walkover;

        public List<Round> Rounds = new List<Round>();

        public List<Match> All_matches()
        {
            return Rounds.SelectMany(x => x.Matches).ToList();
        }

        public List<Team> All_teams()
        {
            var home_teams = All_matches().Select(match => match.Home);
            var away_teams = All_matches().Select(match => match.Away);

            return home_teams.Concat(away_teams).ToList();
        }

        public List<Player> All_players()
        {
            var first_players = All_teams().Select(team => team.Player_one);
            var second_players = All_teams().Select(team => team.Player_two);

            return first_players.Concat(second_players).ToList();
        }
    }
}