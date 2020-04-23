using dyp.contracts.data;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.domain
{
    public class PlayWithAllTeamBuilder
    {
        public  IEnumerable<Team> Determine_teams(IEnumerable<Player> players, IEnumerable<(string Player_one, string Player_two)> previous_teams)
        {
            var players_list = players.ToList();
            var teams = new List<Team>();

            while(players_list.Any())
            {
                var team = Determine_team(players_list.First(), players, previous_teams, teams);
                teams.Add(team);
                players_list.RemoveAll(player => player.Id.Equals(team.Member_one.Id) || player.Id.Equals(team.Member_two.Id));
            }

            return teams;
        }

        private Team Determine_team(Player player, IEnumerable<Player> players, 
                                   IEnumerable<(string Player_one, string Player_two)> previous_teams, 
                                   IEnumerable<Team> teams)
        {
            var possible_team_members = Possible_team_members(player.Id, players, teams).ToList();

            var team_members_count_list = Team_members_count(player.Id, possible_team_members, previous_teams).ToList();

            var next_team_member_id = team_members_count_list.OrderBy(item => item.Team_count).First().Player_id;
            var next_team_member = players.First(p => p.Id.Equals(next_team_member_id));

            return new Team() { Member_one = player, Member_two = next_team_member };
        }

        private IEnumerable<string> Possible_team_members(string player_id, IEnumerable<Player> players, IEnumerable<Team> teams)
        {
            var all_other_players = players.Where(player => !player.Id.Equals(player_id));

            var players_in_teams = teams.SelectMany(team => new string[] { team.Member_one.Id, team.Member_two.Id });
            return all_other_players.Where(player => !players_in_teams.Contains(player.Id)).Select(player => player.Id);
        }

        private IEnumerable<(string Player_id, int Team_count)> Team_members_count(string player_id, 
                                                                                  IEnumerable<string> possible_team_members, 
                                                                                  IEnumerable<(string Player_one, string Player_two)> previous_teams)
        {
            var all_teams_from_player = previous_teams.Where(team => team.Player_one.Equals(player_id) || team.Player_two.Equals(player_id)).ToList();

            var Team_members_count_list = new List<(string Player_id, int Team_count)>();
            foreach (var team_member_id in possible_team_members)
            {
                var counter = all_teams_from_player.Count(team => team.Player_one.Equals(team_member_id) || team.Player_two.Equals(team_member_id));
                Team_members_count_list.Add((team_member_id, counter));
            }

            return Team_members_count_list;
        }
    }
}