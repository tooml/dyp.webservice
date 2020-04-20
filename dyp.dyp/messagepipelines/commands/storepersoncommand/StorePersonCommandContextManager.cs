using dyp.contracts.messages.commands.storeperson;
using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.storepersoncommand
{
    public class StorePersonCommandContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;

        public StorePersonCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as StorePersonCommand;
            var person_exist = _es.Replay(typeof(PersonStored)).Any(record =>
            {
                var person_data = record.Data as PersonData;
                return person_data.Person.Id.Equals(cmd.Id);
            });

            return new StorePersonCommandContextModel() { Person_existing = person_exist };
        }

        public void Update(IEnumerable<Event> events) { }
    }
}