using dyp.contracts.data;
using System.Collections.Generic;
using static dyp.contracts.data.Ranking;

namespace dyp.dyp.domain
{
    public class TournamentDirector
    {
        public TournamentDirector() { }

        public Round New_round(IEnumerable<Player> players, int tables)
        {
            var match_generator = new MatchGenerator();
            var matchList = match_generator.Start_match_generation(players, tables);

            var round = new Round
            {
                Matches = matchList.Matches,
                Walkover = matchList.Walkover
            };

            return round;
        }

        public IEnumerable<RankingRow> Calculate_ranking(RankingDataBasis data_basis)
        {
            var ranking_generator = new RankingGenerator();
            return ranking_generator.Generate_ranking(data_basis).Ranking_rows;
        }
    }
}