using dyp.contracts.data;
using dyp.dyp.domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp
{
    public class MatchGenerator
    {
        private const int COMPETITORS_PER_MATCH = 4;

        public MatchGenerator() { }

        public MatchList Start_match_generation(CreateRoundArgs create_parmeter)
        {
            var matches_count = Calculate_matches_count(create_parmeter.Players);
            var players_count_in_round = Players_count_in_round(matches_count);
            var orderd_players = Order_players(create_parmeter.Players).ToList();

            var players_in_round = Extract_players(orderd_players, players_count_in_round);
            var walkover_players = Extract_walkover_players(orderd_players, players_count_in_round);

            var teams = Pull_teams(players_in_round, create_parmeter.Fair_lots, create_parmeter.Previous_teams);
            var matches = Pull_matches(teams, create_parmeter.Fair_lots);
            var assigned_matches = Assign_tables(matches, create_parmeter.Tables);

            return new MatchList() { Matches = assigned_matches, Walkover = walkover_players };
        }

        private int Calculate_matches_count(IEnumerable<Player> players)
        {
            return players.Count() / COMPETITORS_PER_MATCH;
        }

        private int Players_count_in_round(int matches)
        {
            return matches * COMPETITORS_PER_MATCH;
        }

        private IEnumerable<Player> Order_players(IEnumerable<Player> players)
        {
            return players.OrderBy(player => player.Matches).ThenBy(player => player.Walkover_played);
        }

        private IEnumerable<Player> Extract_players(IEnumerable<Player> players, int players_count)
        {
            return players.Take(players_count).ToList();
        }

        private IEnumerable<Player> Extract_walkover_players(IEnumerable<Player> players, int players_count)
        {
            return players.Skip(players_count).ToList();
        }

        private IEnumerable<Team> Pull_teams(IEnumerable<Player> players, bool fair_lots, 
                                             IEnumerable<(string Player_one, string Player_two)> previous_teams)
        {
            var simple_team_builder = new SimpleTeamBuilder();
            var fair_lots_team_builder = new PlayWithAllTeamBuilder();

            switch (fair_lots)
            {
                case true:
                    return fair_lots_team_builder.Determine_teams(players, previous_teams);
                case false:
                    return simple_team_builder.Determine_teams(players);
                default:
                    return simple_team_builder.Determine_teams(players);
            }
        }

        private IEnumerable<Match> Pull_matches(IEnumerable<Team> teams, bool fair_lots)
        {
            switch (fair_lots)
            {
                case true:
                    return Pull_matches_fair(teams);
                case false:
                    return Pull_matches_shuffle(teams);
                default:
                    return Pull_matches_shuffle(teams);
            }
        }

        private IEnumerable<Match> Pull_matches_fair(IEnumerable<Team> teams)
        {
            var teams_by_strength = teams.OrderBy(team => team.Strength).ToArray();
            var pairs = ListPairing.Pairing_list(teams_by_strength);
            var matches = Map(pairs).ToList();

            return matches;
        }

        private IEnumerable<Match> Pull_matches_shuffle(IEnumerable<Team> teams)
        {
            var shuffled_teams = ListShuffle.Shuffle_list(teams.ToArray());
            var pairs = ListPairing.Pairing_list(shuffled_teams);
            var matches = Map(pairs).ToList();

            return matches;
        }

        private IEnumerable<Match> Map(IEnumerable<Tuple<Team, Team>> team_pairs)
        {
            return team_pairs.Select(pair =>
                                            new Match()
                                            {
                                                Home = pair.Item1,
                                                Away = pair.Item2,
                                            });
        }

        private IEnumerable<Match> Assign_tables(IEnumerable<Match> matches, int tables)
        {
            var table = 1;
            matches.ToList().ForEach(match =>
            {
                match.Table = table;
                if (table == tables) table = 1;
                else table++;
            });

            return matches;
        }
    }
}