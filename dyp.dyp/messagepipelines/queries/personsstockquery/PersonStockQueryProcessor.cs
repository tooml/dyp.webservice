using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.personstock;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.personsstockquery
{
    public class PersonStockQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as PersonStockQueryContextModel;
            return new QueryOutput(new PersonStockQueryResult
            {
                Persons =
                                                                    ctx_model.Persons.Select(p =>
                                                                                    Map(p)).ToArray()
            });
        }

        private Person Map(PersonStockQueryContextModel.PersonInfo personInfo)
        {
            return new Person()
            {
                Id = personInfo.Id,
                FirstName = personInfo.First_name,
                LastName = personInfo.Last_name
            };
        }
    }
}