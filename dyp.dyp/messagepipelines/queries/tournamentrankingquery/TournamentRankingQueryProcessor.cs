using dyp.contracts.data;
using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.tournamentranking;
using dyp.dyp.domain;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentrankingquery
{
    public class TournamentRankingQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as TournamentRankingQueryContextModel;

            var tournament_director = new TournamentDirector();
            var ranking_rows = tournament_director.Calculate_ranking(Map(ctx_model));

            var orderd_ranking_rows = ranking_rows.OrderByDescending(row => row.Q1)
                                    .ThenByDescending(row => row.Q2).ToList();

            var rank = 1;
            var result = orderd_ranking_rows.Select(row => Map(row, rank++)).ToList();

            return new QueryOutput(new TournamentRankingQueryResult { Ranking = result.ToArray() });
        }

        private RankingDataBasis Map(TournamentRankingQueryContextModel model)
        {
            var options_data = new RankingDataBasis.RankingOptionsData()
            {
                Points = model.Tournament_options.Points,
                Points_drawn = model.Tournament_options.Points_drawn,
                Drawn = model.Tournament_options.Drawn,
                Walkover = model.Tournament_options.Walkover
            };

            var players_data = model.Players.Select(p => new RankingDataBasis.RankingPlayerData()
            {
                Player_id = p.Id,
                Player_name = p.Name
            }).ToList();

            var match_datas = model.Matches.Select(m => new RankingDataBasis.RankingMatchData()
            {
                Home = Map(m.Home),
                Away = Map(m.Away),
                Drawn = m.Drawn
            }).ToList();

            return new RankingDataBasis()
            {
                Options = options_data,
                Players = players_data,
                Matches = match_datas,
                Walkover_played = model.Walkover
            };
        }

        private RankingDataBasis.RankingMatchData.Team Map(TournamentRankingQueryContextModel.Team ctx_team)
        {
            return new RankingDataBasis.RankingMatchData.Team()
            {
                Player_one_id = ctx_team.Player_one_id,
                Player_two_id = ctx_team.Player_two_id,
                Sets = ctx_team.Sets
            };
        }

        private RankingRow Map(Ranking.RankingRow row, int rank)
        {
            return new RankingRow()
            {
                Rank = rank,
                PlayerName = row.Player_name,
                Matches = row.Matches_played,
                W = row.Wins,
                D = row.Drawn,
                L = row.Loose,
                Points = row.Points,
                Q1 = decimal.Round(row.Q1, 2),
                Q2 = decimal.Round(row.Q2, 2)
            };
        }


    }
}