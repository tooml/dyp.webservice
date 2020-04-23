using dyp.contracts.messages.commands.changeoptions;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.commands.changeoptionscommand
{
    public class ChangeOptionsCommandContextManager : IMessageContextManager
    {
        public IMessageContext Load(IMessage input)
        {
            var cmd = input as ChangeOptionsCommand;
            var ctx_model = new ChangeOptionsCommandContextModel()
            {
                Tables = cmd.Tables,
                Sets = cmd.Sets,
                Points = cmd.Points,
                PointsDrawn = cmd.PointsDrawn,
                Drawn = cmd.Drawn,
                Walkover = cmd.FairLots
            };

            return ctx_model;
        }

        public void Update(IEnumerable<Event> events) { }
    }
}