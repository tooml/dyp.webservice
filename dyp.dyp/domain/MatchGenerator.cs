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

        public MatchList Start_match_generation(IEnumerable<Player> players, int tables)
        {
            var matches_count = Calculate_matches_count(players);
            var players_count_in_round = Players_count_in_round(matches_count);
            var orderd_players = Order_players(players).ToList();

            var players_in_round = Extract_players(orderd_players, players_count_in_round);
            var walkover_players = Extract_walkover_players(orderd_players, players_count_in_round);

            var teams = Pull_teams(players_in_round);
            var matches = Pull_matches(teams);
            var assigned_matches = Assign_tables(matches, tables);

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

        private IEnumerable<Team> Pull_teams(IEnumerable<Player> players)
        {
            var team_builder = new SimpleTeamBuilder();
            return team_builder.Determine_teams(players);
        }

        //private SimpleTeamBuilder Determine_team_building_strategy(Options options)
        //{
        //    switch (options.Fair_lots)
        //    {
        //        case true:
        //            throw new NotImplementedException();
        //        case false:
        //            return new SimpleTeamBuilder();
        //        default:
        //            throw new NotSupportedException();
        //    }
        //}

        private IEnumerable<Match> Pull_matches(IEnumerable<Team> teams)
        {
            var shuffled_teams = ListShuffle.Shuffle_list(teams.ToArray());
            var pairs = ListPairing.Pairing_list(shuffled_teams);
            var matches = Map(pairs).ToList();

            return matches;
        }

        //private int Calculate_max_sets_to_play(int sets_to_win, bool tied)
        //{
        //    if (tied)
        //        return sets_to_win;

        //    return sets_to_win + (sets_to_win - 1);
        //}

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