using dyp.contracts.messages.queries.competitors;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;
using static dyp.contracts.messages.queries.competitors.CompetitorsQueryResult;

namespace dyp.dyp.messagepipelines.queries.competitorsquery
{
    public class CompetitorsQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as CompetitorsQueryContextModel;
            return new QueryOutput(new CompetitorsQueryResult
            {
                Competitors = ctx_model.Persons.Select(p => Map(p)).ToArray()
            });
        }

        private Competitor Map(CompetitorsQueryContextModel.PersonInfo personInfo)
        {
            return new Competitor()
            {
                Id = personInfo.Id,
                Name = string.Concat(personInfo.First_name, ", ", personInfo.Last_name),
                Compete = false,
                Image = personInfo.Image
            };
        }
    }
}