using dyp.contracts.messages.commands.addplayer;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.changeplayerscommand
{
    public class ChangePlayersCommandContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private ChangePlayersCommandContextModel _ctx_model;

        public ChangePlayersCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as ChangePlayersCommand;
            _ctx_model = new ChangePlayersCommandContextModel();

            var tournament_events = _es.Replay(new TournamentContext(cmd.TournamentId, nameof(TournamentContext)),
                                                          typeof(PlayersStored), typeof(PlayerActivityChanged));
            Update(tournament_events);

            var person_events = _es.Replay(typeof(PersonStored), typeof(PersonUpdated));
            Update(person_events);

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events)
          => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case PersonStored ps:
                    var new_person = Map(ps.Data as PersonData);
                    _ctx_model.Persons.Add(new_person);
                    break;

                case PersonUpdated pu:
                    var person_update = Map(pu.Data as PersonData);
                    var update_pers = _ctx_model.Persons.Single(pers => pers.Id.Equals(person_update.Id));
                    update_pers.First_name = person_update.First_name;
                    update_pers.Last_name = person_update.Last_name;
                    break;

                case PlayersStored ps:
                    var player_data = ev.Data as PlayerData;
                    _ctx_model.Tournament_Players.Add(Map(player_data));
                    break;

                case PlayerActivityChanged pc:
                    var player_activity_data = ev.Data as PlayerActivityData;
                    var update_player = _ctx_model.Tournament_Players.Single(player => player.Id.Equals(player_activity_data.Player_id));
                    update_player.Enabled = player_activity_data.Activ;
                    break;
            }
        }

        private ChangePlayersCommandContextModel.Person Map(PersonData data)
        {
            return new ChangePlayersCommandContextModel.Person()
            {
                Id = data.Person.Id,
                First_name = data.Person.First_name,
                Last_name = data.Person.Last_name
            };
        }

        private ChangePlayersCommandContextModel.Player Map(PlayerData data)
        {
            return new ChangePlayersCommandContextModel.Player()
            {
                Id = data.Player.Id,
                Enabled = true
            };
        }
    }
}