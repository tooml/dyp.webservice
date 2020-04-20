using dyp.contracts.messages.queries.personstats;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.personstatsquery
{
    public class PersonStatsQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as PersonStatsQueryContextModel;

            var tournaments_count = ctx_model.Tournaments.Count(t => t.Matches.Any(m =>
                                                            m.Match_result != PersonStatsQueryContextModel.Result.None));

            var matches = ctx_model.Tournaments.SelectMany(t => t.Matches)
                                               .Where(m => m.Match_result != PersonStatsQueryContextModel.Result.None);

            var matches_count = matches.Count();
            var wins = matches.Count(m => m.Match_result == PersonStatsQueryContextModel.Result.Won);
            var looses = matches.Count(m => m.Match_result == PersonStatsQueryContextModel.Result.Loose);
            var drawn = matches.Count(m => m.Match_result == PersonStatsQueryContextModel.Result.Drawn);

            return new QueryOutput(new PersonStatsQueryResult()
            {
                Tournaments = tournaments_count,
                Matches = matches_count,
                Wins = wins,
                Loose = looses,
                Drawn = drawn
            });
        }
    }
}