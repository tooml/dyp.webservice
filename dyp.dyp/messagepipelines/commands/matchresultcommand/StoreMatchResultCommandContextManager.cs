using System.Collections.Generic;
using System.Linq;
using dyp.contracts.messages.commands.matchresult;
using dyp.dyp.events;
using dyp.dyp.events.context;
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
        private StoreMatchResultCommandContextModel _ctx_model;

        public StoreMatchResultCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as MatchResultNotificationCommand;

            _ctx_model = new StoreMatchResultCommandContextModel();
            _ctx_model.Match_id = cmd.MatchId;

            var match_created_ev = _es.Replay(typeof(MatchCreated)).First(record =>
            {
                var data = record.Data as MatchData;
                return data.Id.Equals(cmd.MatchId);
            });

            Update(new Event[] { match_created_ev });


            var player_strength_ev = _es.Replay(new TournamentContext(_ctx_model.Tournament_id, nameof(TournamentContext)),
                                                typeof(PlayerStrengthChanged)).Where(record =>
            {
                var data = record.Data as PlayerStrengthData;
                return data.Match_id.Equals(cmd.MatchId);
            });


            Update(player_strength_ev);

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events)
         => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case MatchCreated mc:
                    var match = mc.Data as MatchData;
                    _ctx_model.Tournament_id = mc.Context.Id;
                    _ctx_model.Home_player = new string[] { match.Home.Player_one.Id, match.Home.Player_two.Id };
                    _ctx_model.Away_player = new string[] { match.Away.Player_one.Id, match.Away.Player_two.Id };
                    break;

                case PlayerStrengthChanged sc:
                    var strength_data = sc.Data as PlayerStrengthData;
                    _ctx_model.Update_strength(strength_data.Player_id, strength_data.Strength_amount);
                    break;
            }
        }
    }
}