using System.Collections.Generic;
using System.Linq;
using dyp.contracts.messages.queries.tournamentround;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.queries.tournamentroundquery
{
    public class TournamentRoundQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private TournamentRoundQueryContextModel _ctx_model;

        public TournamentRoundQueryContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            _ctx_model = new TournamentRoundQueryContextModel();
            var query = input as TournamentRoundQuery;
            var events = _es.Replay(new TournamentContext(query.TournamentId, nameof(TournamentContext)),
                                                         typeof(RoundCreated), typeof(MatchCreated),
                                                         typeof(MatchPlayed), typeof(MatchReseted));

            var round_event = events.Where(e => e is RoundCreated).Last();
            var match_events = events.Where(e => e is MatchCreated);
            var match_played_events = events.Where(e => e is MatchPlayed || e is MatchReseted);

            Apply(round_event as RoundCreated);
            foreach (var ev in match_events) { Apply(ev as MatchCreated); }
            foreach (var ev in match_played_events) { Apply(ev); }

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events) { }

        private void Apply(RoundCreated ev)
        {
            var data = ev.Data as RoundData;
            _ctx_model.Tournament_Id = ev.Id;
            _ctx_model.Id = data.Id;
            _ctx_model.Count = data.Count;
        }

        private void Apply(MatchCreated ev)
        {
            var data = ev.Data as MatchData;

            var match = new TournamentRoundQueryContextModel.Match();
            match.Id = data.Id;
            match.Table = data.Table;
            match.Sets = data.Sets;
            match.Drawn = data.Drawn;
            match.Results = Enumerable.Range(1, match.Sets).Select(set_number => (TournamentRoundQueryContextModel.SetResult)SetResult.None).ToList();

            match.Home = new TournamentRoundQueryContextModel.Team();
            match.Home.Player_one = Map_player(data.Home.Player_one);
            match.Home.Player_two = Map_player(data.Home.Player_two);

            match.Away = new TournamentRoundQueryContextModel.Team();
            match.Away.Player_one = Map_player(data.Away.Player_one);
            match.Away.Player_two = Map_player(data.Away.Player_two);

            if (_ctx_model.Id.Equals(data.Round_id))
                _ctx_model.Matches.Add(match);
        }

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case MatchPlayed mp:
                    var m_played_data = ev.Data as MatchResultData;
                    var match_results = m_played_data.Results.Select(result =>
                                                            (TournamentRoundQueryContextModel.SetResult)result).ToList();

                    Update_match_result(m_played_data.Match_id, match_results);
                    break;

                case MatchReseted mr:
                    var match_reset_data = ev.Data as MatchResetData;
                    var sets_count = _ctx_model.Matches.Any(match => match.Id.Equals(match_reset_data.Match_id)) ?
                                        _ctx_model.Matches.First(match => match.Id.Equals(match_reset_data.Match_id)).Sets : 0;
                    var sets = _ctx_model.Create_default_sets(sets_count).ToList();

                    Update_match_result(match_reset_data.Match_id, sets);
                    break;
            }
        }

        private TournamentRoundQueryContextModel.Player Map_player(Player player)
        {
            return new TournamentRoundQueryContextModel.Player()
            {
                Id = player.Id,
                First_name = player.First_name,
                Last_name = player.Last_name,
                Image = player.Image
            };
        }

        private void Update_match_result(string match_id, IEnumerable<TournamentRoundQueryContextModel.SetResult> setResults)
        {
            if (_ctx_model.Matches.Exists(m => m.Id.Equals(match_id)))
            {
                var match = _ctx_model.Matches.First(m => m.Id.Equals(match_id));
                match.Results = setResults.ToList();
            }
        }
    }
}