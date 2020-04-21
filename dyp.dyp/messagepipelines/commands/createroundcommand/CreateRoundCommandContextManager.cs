using dyp.contracts.data;
using dyp.contracts.messages.commands.createnewround;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.createroundcommand
{
    public class CreateRoundCommandContextManager : IMessageContextManager
    {
        private CreateRoundCommandContextModel _ctx_model;
        private readonly IEventStore _es;

        public CreateRoundCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as CreateRoundCommand;

            _ctx_model = new CreateRoundCommandContextModel();
            _ctx_model.Players = new List<CreateRoundCommandContextModel.Player>();
            _ctx_model.Walkover_player_ids = new List<string>();

            var events = _es.Replay(new TournamentContext(cmd.TournamentId, nameof(TournamentContext)),
                typeof(RoundCreated), typeof(PlayersStored), typeof(OptionsCreated),
                typeof(WalkoverPlayed), typeof(MatchCreated), typeof(PlayerActivityChanged));
            Update(events);

            var person_events = _es.Replay(typeof(PersonDeleted));
            Update(person_events);

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events)
           => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case PlayersStored ps:
                    var player_data = ev.Data as PlayerData;
                    _ctx_model.Players.Add(new CreateRoundCommandContextModel.Player()
                    {
                        Id = player_data.Player.Id,
                        First_name = player_data.Player.First_name,
                        Last_name = player_data.Player.Last_name,
                        Image = player_data.Player.Image,
                        Enabled = true
                    });
                    break;

                case OptionsCreated os:
                    var options_data = ev.Data as OptionsData;
                    _ctx_model.Tables = options_data.Tables;
                    _ctx_model.Sets = options_data.Sets;
                    _ctx_model.Drawn = options_data.Drawn;
                    _ctx_model.Walkover = options_data.Walkover;
                    break;

                case WalkoverPlayed wp:
                    var walkover_data = ev.Data as WalkoverData;
                    _ctx_model.Walkover_player_ids.Add(walkover_data.Id);
                    break;

                case MatchCreated mc:
                    var match_data = ev.Data as MatchData;
                    _ctx_model.Players.Single(player => player.Id.Equals(match_data.Home.Player_one.Id)).Matches++;
                    _ctx_model.Players.Single(player => player.Id.Equals(match_data.Home.Player_two.Id)).Matches++;
                    _ctx_model.Players.Single(player => player.Id.Equals(match_data.Away.Player_one.Id)).Matches++;
                    _ctx_model.Players.Single(player => player.Id.Equals(match_data.Away.Player_two.Id)).Matches++;
                    break;

                case RoundCreated rc:
                    _ctx_model.Round_count++;
                    break;

                case PlayerActivityChanged pc:
                    var player_activity_data = ev.Data as PlayerActivityData;
                    var update_player = _ctx_model.Players.Single(player => player.Id.Equals(player_activity_data.Player_id));
                    update_player.Enabled = player_activity_data.Activ;
                    break;

                case PersonDeleted ps:
                    var person_delete_data = ps.Data as PersonDeleteData;
                    var index = _ctx_model.Players.FindIndex(person => person.Id.Equals(person_delete_data.Id));
                    _ctx_model.Players.RemoveAt(index);
                    break;
            }
        }
    }
}