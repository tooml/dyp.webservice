using dyp.contracts.messages.queries.tournament;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentquery
{
    public class TournamentQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private TournamentQueryContextModel _ctx_model;

        public TournamentQueryContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            _ctx_model = new TournamentQueryContextModel();
            var query = input as TournamentQuery;
            var tournament_events = _es.Replay(new TournamentContext(query.TournamentId, nameof(TournamentContext)));
            var person_events = _es.Replay(typeof(PersonUpdated));

            var tournament_event = tournament_events.First(e => e is TournamentCreated);
            var option_events = tournament_events.Where(e => e is OptionsCreated);
            var round_events = tournament_events.Where(e => e is RoundCreated);
            var match_events = tournament_events.Where(e => e is MatchCreated);
            var match_played_events = tournament_events.Where(e => e is MatchPlayed || e is MatchReseted);

            Apply(tournament_event as TournamentCreated);
            foreach (var ev in option_events) { Apply(ev as OptionsCreated); }
            foreach (var ev in round_events) { Apply(ev as RoundCreated); }
            foreach (var ev in match_events) { Apply(ev as MatchCreated); }
            foreach (var ev in match_played_events) { Apply(ev); }
            foreach (var ev in person_events) { Apply(ev as PersonUpdated); }

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events) { }

        private void Apply(TournamentCreated ev)
        {
            var data = ev.Data as TournamentData;
            _ctx_model.Id = data.Id;
            _ctx_model.Name = data.Name;
            _ctx_model.Created = data.Created;
        }

        private void Apply(OptionsCreated ev)
        {
            var data = ev.Data as OptionsData;
            _ctx_model.Tables = data.Tables;
            _ctx_model.Sets = data.Sets;
            _ctx_model.Points = data.Points;
            _ctx_model.Points_drawn = data.Points_drawn;
            _ctx_model.Drawn = data.Drawn;
            _ctx_model.Walkover = data.Fair_lots;
        }

        private void Apply(RoundCreated ev)
        {
            var data = ev.Data as RoundData;
            var round = new TournamentQueryContextModel.Round();
            round.Id = data.Id;
            round.Count = data.Count;
            _ctx_model.Rounds.Add(round);
        }

        private void Apply(MatchCreated ev)
        {
            var data = ev.Data as MatchData;

            var match = new TournamentQueryContextModel.Match();
            match.Id = data.Id;
            match.Table = data.Table;
            match.Sets = data.Sets;
            match.Drawn = data.Drawn;
            match.Results = _ctx_model.Create_default_sets(match.Sets).ToList();

            match.Home = new TournamentQueryContextModel.Team();
            match.Home.Player_one = Map_player(data.Home.Player_one);
            match.Home.Player_two = Map_player(data.Home.Player_two);

            match.Away = new TournamentQueryContextModel.Team();
            match.Away.Player_one = Map_player(data.Away.Player_one);
            match.Away.Player_two = Map_player(data.Away.Player_two);

            var round = _ctx_model.Rounds.First(r => r.Id.Equals(data.Round_id));
            round.Matches.Add(match);
        }

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case MatchPlayed mp:
                    var m_played_data = ev.Data as MatchResultData;
                    var match_results = m_played_data.Results.Select(result => (TournamentQueryContextModel.SetResult)result).ToList();
                    var ctx_model_match = _ctx_model.All_matches().First(m => m.Id.Equals(m_played_data.Match_id));

                    ctx_model_match.Results = match_results;
                    break;
                case MatchReseted mr:
                    var match_reset_data = ev.Data as MatchResetData;
                    var ctx_model_match_reset = _ctx_model.All_matches().First(m => m.Id.Equals(match_reset_data.Match_id));

                    ctx_model_match_reset.Results = _ctx_model.Create_default_sets(ctx_model_match_reset.Sets).ToList();
                    break;
            }
        }

        private TournamentQueryContextModel.Player Map_player(events.data.Player player)
        {
            return new TournamentQueryContextModel.Player()
            {
                Id = player.Id,
                First_name = player.First_name,
                Last_name = player.Last_name
            };
        }

        private void Apply(PersonUpdated ev)
        {
            var data = ev.Data as PersonData;
            var players = _ctx_model.All_players().Where(x => x.Id == data.Person.Id);
            foreach (var player in players)
            {
                player.First_name = data.Person.First_name;
                player.Last_name = data.Person.Last_name;
            }
        }
    }
}