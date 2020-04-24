using dyp.contracts.messages.queries.tournamentplayers;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentplayersquery
{
    public class TournamentCompetitorsQueryContextProcessor : IMessageProcessor
    {
        public Output Process(IMessage _, IMessageContext model)
        {
            var ctx_model = model as TournamentCompetitorsQueryContextModel;
            var competitors = ctx_model.Competitors.Select(p => Map(p)).ToArray();

            return new QueryOutput(new TournamentCompetitorsQueryResult() { Competitors = competitors });
        }

        private TournamentCompetitorsQueryResult.Competitor Map(TournamentCompetitorsQueryContextModel.Competitor competitor)
        {
            return new TournamentCompetitorsQueryResult.Competitor()
            {
                Id = competitor.Id,
                Name = string.Concat(competitor.First_name, ", ", competitor.Last_name),
                Compete = competitor.Enabled
            };
        }
    }
}