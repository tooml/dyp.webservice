using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.tournament;
using dyp.dyp.messagepipelines.mapping;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentquery
{
    public class TournamentQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as TournamentQueryContextModel;

            var tournament = Map(ctx_model);
            return new QueryOutput(new TournamentQueryResult { Tournament = tournament });
        }

        private Tournament Map(TournamentQueryContextModel ctx_model)
        {
            return new Tournament()
            {
                Id = ctx_model.Id,
                Name = ctx_model.Name,
                Created = ctx_model.Created,
                Options = new Options()
                {
                    Tables = ctx_model.Tables,
                    Sets = ctx_model.Sets,
                    Points = ctx_model.Points,
                    PointsDrawn = ctx_model.Points_drawn,
                    Drawn = ctx_model.Drawn,
                    Walkover = ctx_model.Walkover
                },

                Rounds = ctx_model.Rounds.Select(r => TournamentMapper.Map(r, ctx_model.Id))
            };
        }
    }
}