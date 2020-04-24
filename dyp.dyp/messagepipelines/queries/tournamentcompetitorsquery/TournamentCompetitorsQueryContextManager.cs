using dyp.contracts.messages.queries.tournamentplayers;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentplayersquery
{
    public class TournamentCompetitorsQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private TournamentCompetitorsQueryContextModel _ctx_model;

        public TournamentCompetitorsQueryContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var query = input as TournamentCompetitorsQuery;
            _ctx_model = new TournamentCompetitorsQueryContextModel();

            var person_created_events = _es.Replay(typeof(PersonStored));
            Update(person_created_events);

            var person_updated_events = _es.Replay(typeof(PersonUpdated));
            Update(person_updated_events);

            var events = _es.Replay(new TournamentContext(query.TournamentId, nameof(TournamentContext)),
                                                          typeof(PlayersStored), typeof(PlayerActivityChanged));
            Update(events);

            var person_deleted_events = _es.Replay(typeof(PersonDeleted));
            Update(person_deleted_events);

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
                    _ctx_model.Competitors.Add(new_person);
                    break;

                case PersonUpdated pu:
                    var person_update = Map(pu.Data as PersonData);
                    var update_pers = _ctx_model.Competitors.Single(pers => pers.Id.Equals(person_update.Id));
                    update_pers.First_name = person_update.First_name;
                    update_pers.Last_name = person_update.Last_name;
                    break;

                case PlayersStored ps:
                    var player_data = ev.Data as PlayerData;
                    var competitor = _ctx_model.Competitors.Find(c => c.Id.Equals(player_data.Player.Id));
                    competitor.Enabled = true;
                    break;

                case PlayerActivityChanged pc:
                    var player_activity_data = ev.Data as PlayerActivityData;
                    var _competitor = _ctx_model.Competitors.Find(c => c.Id.Equals(player_activity_data.Player_id));
                    _competitor.Enabled = player_activity_data.Activ;
                    break;

                case PersonDeleted ps:
                    var person_delete_data = ps.Data as PersonDeleteData;
                    var index = _ctx_model.Competitors.FindIndex(pers => pers.Id.Equals(person_delete_data.Id));
                    _ctx_model.Competitors.RemoveAt(index);
                    break;
            }
        }

        private TournamentCompetitorsQueryContextModel.Competitor Map(PersonData data)
        {
            return new TournamentCompetitorsQueryContextModel.Competitor()
            {
                Id = data.Person.Id,
                First_name = data.Person.First_name,
                Last_name = data.Person.Last_name,
                Enabled = false
            };
        }
    }
}