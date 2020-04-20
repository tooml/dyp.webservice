using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.tournamentround;
using dyp.dyp.messagepipelines.mapping;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentroundquery
{
    public class TournamentRoundQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as TournamentRoundQueryContextModel;

            var round = new Round()
            {
                Id = ctx_model.Id,
                TournamentId = ctx_model.Tournament_Id,
                Name = TournamentMapper.Round_name(ctx_model.Count),
                Matches = ctx_model.Matches.Select(match => TournamentMapper.Map(match))
            };

            return new QueryOutput(new TournamentRoundQueryResult() { Round = round });
        }
    }
}