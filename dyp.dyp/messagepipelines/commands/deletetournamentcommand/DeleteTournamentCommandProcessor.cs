using dyp.contracts.messages.commands.deletetournamentcommand;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.deletetournamentcommand
{
    public class DeleteTournamentCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as DeleteTournamentCommand;
            var ctx_model = model as DeleteTournamentCommandContextModel;

            if (ctx_model.Tournament_existing)
            {
                var tournament_context = new TournamentContext(cmd.Id, nameof(TournamentContext));
                var ev = new TournamentDeleted(nameof(TournamentDeleted), tournament_context, new TournamentDeleteData() { Id = cmd.Id });
                return new CommandOutput(new Success(), new Event[] { ev });
            }

            return new CommandOutput(new Failure("Turnier existiert nicht"));
        }
    }
}