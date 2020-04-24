using dyp.contracts.messages.queries.data;
using dyp.contracts.messages.queries.personavtars;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.personavatarsquery
{
    public class PersonAvtarsQueryProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as PersonAvtarsQueryContextModel;

            return new QueryOutput(new PersonAvatarsQueryResult
            {
                Avatars = Map(ctx_model.Avatars).ToList()
            });
        }

        private IEnumerable<PersonAvatar> Map(IEnumerable<PersonAvtarsQueryContextModel.PersonAvatar> datas) =>
             datas.Select(data => new PersonAvatar() { PersonId = data.Person_id, Avatar = data.Avatar });
    }
}