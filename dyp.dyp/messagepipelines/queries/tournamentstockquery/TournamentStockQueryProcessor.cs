using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.tournamentstock;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentstockquery
{
    public class TournamentStockQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as TournamentStockQueryContextModel;
            return new QueryOutput(new TournamentStockQueryResult
            {
                Tournaments = ctx_model.Tournaments.Select(t => Map(t))
                                                    .OrderByDescending(t => DateTime.Parse(t.Created)).ToList()
            });
        }

        private Tournament Map(TournamentStockQueryContextModel.Tournament t)
        {
            return new Tournament() { Id = t.Id, Name = t.Name, Created = t.Created };
        }
    }
}