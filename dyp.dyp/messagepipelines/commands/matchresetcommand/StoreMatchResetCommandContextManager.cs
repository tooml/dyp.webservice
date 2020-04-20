using dyp.contracts.messages.commands.matchreset;
using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.matchresetcommand
{
    public class StoreMatchResetCommandContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;

        public StoreMatchResetCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as MatchResetCommand;
            var ev = _es.Replay(typeof(MatchCreated)).First(record =>
            {
                var match_data = record.Data as MatchData;
                return match_data.Id.Equals(cmd.MatchId);
            });

            var match = ev.Data as MatchData;
            return new StoreMatchResetCommandContextModel()
            {
                Tournament_id = ev.Context.Id,
                Match_id = match.Id,
                Player_ids = new string[] { match.Home.Player_one.Id, match.Home.Player_two.Id,
                                            match.Away.Player_one.Id, match.Away.Player_two.Id }
            };
        }

        public void Update(IEnumerable<Event> events) { }
    }
}