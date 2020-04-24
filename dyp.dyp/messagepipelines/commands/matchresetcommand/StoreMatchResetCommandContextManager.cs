using dyp.contracts.messages.commands.matchreset;
using dyp.dyp.events;
using dyp.dyp.events.context;
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
        private StoreMatchResetCommandContextModel _ctx_model;
        public StoreMatchResetCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as MatchResetCommand;
            var match_created_ev = _es.Replay(typeof(MatchCreated)).First(record =>
            {
                var match_data = record.Data as MatchData;
                return match_data.Id.Equals(cmd.MatchId);
            });

            _ctx_model = new StoreMatchResetCommandContextModel() { Tournament_id = match_created_ev.Context.Id };
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
                    var match_created_data = mc.Data as MatchData;
                    _ctx_model.Match_id = match_created_data.Id;
                    _ctx_model.Home_player = new string[] { match_created_data.Home.Player_one.Id, match_created_data.Home.Player_two.Id };
                    _ctx_model.Away_player = new string[] { match_created_data.Away.Player_one.Id, match_created_data.Away.Player_two.Id };
                    break;

                case PlayerStrengthChanged sc:
                    var player_strength_data = sc.Data as PlayerStrengthData;
                    _ctx_model.Update_strength(player_strength_data.Player_id, player_strength_data.Strength_amount);
                    break;
            }
        }
    }
}