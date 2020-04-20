using dyp.contracts.messages.queries.tournamentranking;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.tournamentrankingquery
{
    public class TournamentRankQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private TournamentRankingQueryContextModel _ctx_model;

        public TournamentRankQueryContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            _ctx_model = new TournamentRankingQueryContextModel();
            var query = input as TournamentRankingQuery;
            var tournament_events = _es.Replay(new TournamentContext(query.TournamentId, nameof(TournamentContext)));
            var person_events = _es.Replay(typeof(PersonUpdated));

            var tournament_option_events = tournament_events.Where(e => e is OptionsCreated);
            var players_events = tournament_events.Where(e => e is PlayersStored);
            var match_events = tournament_events.Where(e => e is MatchCreated);
            var match_played_events = tournament_events.Where(e => e is MatchPlayed || e is MatchReseted);
            var walkover_played_events = tournament_events.Where(e => e is WalkoverPlayed);

            foreach (var ev in tournament_option_events) { Apply(ev as OptionsCreated); }
            foreach (var ev in players_events) { Apply(ev as PlayersStored); }
            foreach (var ev in match_events) { Apply(ev as MatchCreated); }
            foreach (var ev in match_played_events) { Apply(ev); }
            foreach (var ev in walkover_played_events) { Apply(ev as WalkoverPlayed); }
            foreach (var ev in person_events) { Apply(ev as PersonUpdated); }

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events) { }

        private void Apply(OptionsCreated ev)
        {
            var data = ev.Data as OptionsData;
            _ctx_model.Tournament_options = new TournamentRankingQueryContextModel.Options()
            {
                Points = data.Points,
                Points_drawn = data.Points_drawn,
                Drawn = data.Drawn,
                Walkover = data.Walkover
            };
        }

        private void Apply(PlayersStored ev)
        {
            var data = ev.Data as PlayerData;
            var player = new TournamentRankingQueryContextModel.Player()
            {
                Id = data.Player.Id,
                Name = string.Concat(data.Player.First_name, ' ', data.Player.Last_name.First(), '.')
            };

            _ctx_model.Players.Add(player);
        }

        private void Apply(MatchCreated ev)
        {
            var data = ev.Data as MatchData;

            var match = new TournamentRankingQueryContextModel.Match();
            match.Id = data.Id;
            match.Home = Map_team(data.Home.Player_one.Id, data.Home.Player_two.Id, 0);
            match.Away = Map_team(data.Away.Player_one.Id, data.Away.Player_two.Id, 0);
            match.Drawn = data.Drawn;

            _ctx_model.Matches.Add(match);
        }

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case MatchPlayed mp:
                    var m_played_data = ev.Data as MatchResultData;
                    var match = _ctx_model.Matches.First(m => m.Id.Equals(m_played_data.Match_id));
                    match.Home.Sets = m_played_data.Results.Count(result => result.Equals(SetResult.Home) || result.Equals(SetResult.Drawn));
                    match.Away.Sets = m_played_data.Results.Count(result => result.Equals(SetResult.Away) || result.Equals(SetResult.Drawn));
                    break;
                case MatchReseted mr:
                    var match_reset_data = ev.Data as MatchResetData;
                    var match_reset = _ctx_model.Matches.First(m => m.Id.Equals(match_reset_data.Match_id));
                    match_reset.Home.Sets = 0;
                    match_reset.Away.Sets = 0;
                    break;
            }
        }

        private void Apply(WalkoverPlayed ev)
        {
            var data = ev.Data as WalkoverData;
            _ctx_model.Walkover.Add(data.Id);
        }

        private void Apply(PersonUpdated ev)
        {
            var data = ev.Data as PersonData;
            if (_ctx_model.Players.Exists(p => p.Id.Equals(data.Person.Id)))
                _ctx_model.Players.First(p => p.Id.Equals(data.Person.Id)).Name =
                    string.Concat(data.Person.First_name, ' ', data.Person.Last_name.First(), '.');
        }

        private TournamentRankingQueryContextModel.Team Map_team(string player_one_id, string player_two_id, int sets)
        {
            return new TournamentRankingQueryContextModel.Team()
            {
                Player_one_id = player_one_id,
                Player_two_id = player_two_id,
                Sets = sets
            };
        }
    }
}