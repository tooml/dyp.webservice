using dyp.contracts.messages.commands.changeoptions;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.changeoptionscommand
{
    public class ChangeOptionsCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as ChangeOptionsCommand;
            var ctx_model = model as ChangeOptionsCommandContextModel;

            var ev = Map(ctx_model, cmd);

            return new CommandOutput(new Success(), new Event[] { ev });
        }

        private Event Map(ChangeOptionsCommandContextModel ctx_model, ChangeOptionsCommand cmd)
        {
            var options_data = new OptionsData()
            {
                Tables = ctx_model.Tables,
                Sets = ctx_model.Sets,
                Points = ctx_model.Points,
                Points_drawn = ctx_model.PointsDrawn,
                Drawn = ctx_model.Drawn,
                Walkover = ctx_model.Walkover
            };

            return new OptionsCreated(nameof(OptionsCreated),
                new TournamentContext(cmd.TournamentId, nameof(TournamentContext)), options_data);
        }
    }
}