using dyp.contracts.messages.commands.storeperson;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.storepersoncommand
{
    public class StorePersonCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as StorePersonCommand;
            var ctx_model = model as StorePersonCommandContextModel;

            var ev = Map(ctx_model, cmd);
            return new CommandOutput(new Success(), new Event[] { ev });
        }

        private Event Map(StorePersonCommandContextModel cmd_model, StorePersonCommand cmd)
        {
            var person_data = new PersonData()
            {
                Person = new Person()
                {
                    Id = cmd.Id,
                    First_name = cmd.FirstName,
                    Last_name = cmd.LastName,
                    Image = string.IsNullOrEmpty(cmd.Image) ? string.Empty : cmd.Image
                }
            };

            var person_context = new PersonsContext(person_data.Person.Id, nameof(PersonsContext));

            if (cmd_model.Person_existing)
                return new PersonUpdated(nameof(PersonUpdated), person_context, person_data);

            return new PersonStored(nameof(PersonStored), person_context, person_data);
        }
    }
}