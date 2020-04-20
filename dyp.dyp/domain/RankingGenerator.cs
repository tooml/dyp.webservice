using dyp.contracts.data;
using System;
using System.Collections.Generic;
using System.Linq;
using static dyp.contracts.data.RankingDataBasis;

namespace dyp.dyp.domain
{
    public class RankingGenerator
    {
        public Ranking Generate_ranking(RankingDataBasis data_basis)
        {
            var ranking = new Ranking(data_basis.Players);
            var matches = Filter_valid_matches(data_basis.Matches).ToList();
            Apply_to_match(matches, match => Fill_matches_count(ranking, match));
            Apply_to_match(matches, match => Fill_matches_stats(ranking, match, data_basis.Options));
            Fill_walkovers(ranking, data_basis.Walkover_played);
            Apply_to_match(matches, match => Fill_Q2(ranking, match));

            return ranking;
        }

        private IEnumerable<RankingMatchData> Filter_valid_matches(IEnumerable<RankingMatchData> matches)
        {
            return matches.Where(match => match.Home.Sets > 0 || match.Away.Sets > 0).ToList();
        }

        private void Apply_to_match(IEnumerable<RankingMatchData> matches, Action<RankingMatchData> apply)
        {
            matches.ToList().ForEach(match => apply(match));
        }

        private void Fill_matches_count(Ranking ranking, RankingMatchData match)
        {
            var players = match.Get_match_players();
            ranking.Apply_match_count(players);
        }

        private void Fill_matches_stats(Ranking ranking, RankingMatchData match, RankingOptionsData options_data)
        {
            ranking.Apply_match_win(match.Get_match_winners(), options_data.Points);
            ranking.Apply_match_drawn(match.Get_match_drawn(), options_data.Points_drawn);
            ranking.Apply_match_loose(match.Get_match_loosers());
        }

        private void Fill_walkovers(Ranking ranking, IEnumerable<string> walkover_players)
        {
            ranking.Apply_walkover(walkover_players);
        }

        private void Fill_Q2(Ranking ranking, RankingMatchData match)
        {
            var home_players = match.Get_match_home_players();
            var away_players = match.Get_match_away_players();

            var home_players_q1_avarage = ranking.Get_players_Q1(home_players) / 2;
            var away_players_q1_avarage = ranking.Get_players_Q1(away_players) / 2;

            ranking.Apply_Q2_total(home_players, away_players_q1_avarage);
            ranking.Apply_Q2_total(away_players, home_players_q1_avarage);
        }
    }
}