using dyp.contracts.messages.commands.deleteperson;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.deletepersoncommand
{
    public class DeletePersonCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as DeletePersonCommand;
            var ctx_model = model as DeletePersonCommandContextModel;

            if (ctx_model.Person_existing)
            {
                var person_context = new PersonsContext(cmd.Id, nameof(PersonsContext));
                var ev = new PersonDeleted(nameof(PersonDeleted), person_context, new PersonDeleteData() { Id = cmd.Id });
                return new CommandOutput(new Success(), new Event[] { ev });
            }

            return new CommandOutput(new Failure("Person existiert nicht"));
        }
    }
}