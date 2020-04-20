using dyp.contracts.messages.queries.personstats;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.personstatsquery
{
    public class PersonStatsQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private PersonStatsQueryContextModel _ctx_model;

        public PersonStatsQueryContextManager(IEventStore es) { _es = es; }
        public IMessageContext Load(IMessage input)
        {
            _ctx_model = new PersonStatsQueryContextModel();
            var query = input as PersonStatsQuery;
            var events = _es.Replay(new PersonsContext(query.PersonId, nameof(PersonsContext)),
                                                         typeof(PersonMatchStatsCreated), typeof(PersonMatchStatsDeleted));

            foreach (var ev in events)
            {
                Prepare_model(ev);
                Apply(ev);
            }

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events) { }

        private void Prepare_model(Event ev)
        {
            switch (ev)
            {
                case PersonMatchStatsCreated sc:
                    var data = ev.Data as PersonMatchStatsData;
                    if (!_ctx_model.Tournaments.Exists(t => t.Id.Equals(data.Tournament_id)))
                        _ctx_model.Tournaments.Add(Init_Tournament(data.Tournament_id));

                    if (!_ctx_model.Tournaments.First(t => t.Id.Equals(data.Tournament_id))
                                               .Matches.Exists(m => m.Id.Equals(data.Match_id)))
                        _ctx_model.Tournaments.First(t => t.Id.Equals(data.Tournament_id))
                                             .Matches.Add(Init_Match(data.Match_id, (PersonStatsQueryContextModel.Result)data.Match_result));
                    break;
            }
        }

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case PersonMatchStatsCreated sc:
                    var created_stats = ev.Data as PersonMatchStatsData;
                    var tournament = _ctx_model.Tournaments.First(t => t.Id.Equals(created_stats.Tournament_id));
                    var match = tournament.Matches.First(ma => ma.Id.Equals(created_stats.Match_id));
                    match.Match_result = (PersonStatsQueryContextModel.Result)created_stats.Match_result;
                    break;

                case PersonMatchStatsDeleted sd:
                    var delete_stats = ev.Data as PersonMatchStatsData;
                    var match_to_delete = _ctx_model.Tournaments.First(t => t.Id.Equals(delete_stats.Tournament_id))
                                                                .Matches.First(m => m.Id.Equals(delete_stats.Match_id));
                    match_to_delete.Match_result = PersonStatsQueryContextModel.Result.None;
                    break;
            }
        }

        private PersonStatsQueryContextModel.Tournament Init_Tournament(string tournament_id)
        {
            return new PersonStatsQueryContextModel.Tournament()
            {
                Id = tournament_id,
                Matches = new List<PersonStatsQueryContextModel.Match>()
            };
        }

        private PersonStatsQueryContextModel.Match Init_Match(string match_id, PersonStatsQueryContextModel.Result match_result)
        {
            return new PersonStatsQueryContextModel.Match()
            {
                Id = match_id,
                Match_result = match_result
            };
        }
    }
}