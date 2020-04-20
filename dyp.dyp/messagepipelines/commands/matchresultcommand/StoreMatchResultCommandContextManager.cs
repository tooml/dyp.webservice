using System.Collections.Generic;
using System.Linq;
using dyp.contracts.messages.commands.matchresult;
using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.matchresultcommand
{
    public class StoreMatchResultCommandContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;

        public StoreMatchResultCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as MatchResultNotificationCommand;
            var ev = _es.Replay(typeof(MatchCreated)).First(record =>
            {
                var match_data = record.Data as MatchData;
                return match_data.Id.Equals(cmd.MatchId);
            });

            var match = ev.Data as MatchData;
            return new StoreMatchResultCommandContextModel()
            {
                Tournament_id = ev.Context.Id,
                Home_player_ids = new string[] { match.Home.Player_one.Id, match.Home.Player_two.Id },
                Away_player_ids = new string[] { match.Away.Player_one.Id, match.Away.Player_two.Id },
            };
        }

        public void Update(IEnumerable<Event> events) { }
    }
}