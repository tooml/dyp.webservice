using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;
using static dyp.dyp.messagepipelines.queries.tournamentstockquery.TournamentStockQueryContextModel;

namespace dyp.dyp.messagepipelines.queries.tournamentstockquery
{
    public class TournamentStockQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private readonly List<Tournament> _tournaments;

        public TournamentStockQueryContextManager(IEventStore es)
        {
            _es = es;
            _tournaments = new List<Tournament>();
        }

        public IMessageContext Load(IMessage input)
        {
            var model = new TournamentStockQueryContextModel();
            var events = _es.Replay(typeof(TournamentCreated), typeof(TournamentDeleted));

            Update(events);
            model.Tournaments = _tournaments;
            return model;
        }

        public void Update(IEnumerable<Event> events)
         => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case TournamentCreated tc:
                    var tournament = Map_tournament_data(tc.Data as TournamentData);
                    _tournaments.Add(tournament);
                    break;
                case TournamentDeleted td:
                    var tournament_data = (td.Data as TournamentDeleteData);
                    var index = _tournaments.FindIndex(t => t.Id.Equals(tournament_data.Id));
                    _tournaments.RemoveAt(index);
                    break;
            }
        }

        private Tournament Map_tournament_data(TournamentData eventData)
        {
            return new Tournament()
            {
                Id = eventData.Id,
                Name = eventData.Name,
                Created = eventData.Created
            };
        }
    }
}