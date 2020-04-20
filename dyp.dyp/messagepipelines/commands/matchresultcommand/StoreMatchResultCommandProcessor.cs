using dyp.contracts.messages.commands.matchresult;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;
using SetResult = dyp.contracts.messages.commands.matchresult.SetResult;

namespace dyp.dyp.messagepipelines.commands.matchresultcommand
{
    public class StoreMatchResultCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as MatchResultNotificationCommand;
            var ctx_model = model as StoreMatchResultCommandContextModel;
            var match_result_ev = Map_match_result(ctx_model, cmd);
            var person_stats_ev = Map_person_stats(ctx_model, cmd);

            var events = new List<Event>(person_stats_ev);
            events.Add(match_result_ev);

            return new CommandOutput(new Success(), events.ToArray());
        }

        private Event Map_match_result(StoreMatchResultCommandContextModel ctx_model, MatchResultNotificationCommand cmd)
        {
            var match_result_data = new MatchResultData()
            {
                Match_id = cmd.MatchId,
                Results = cmd.Results.Select(result => (events.data.SetResult)result).ToList()
            };

            return new MatchPlayed(nameof(MatchPlayed),
                new TournamentContext(ctx_model.Tournament_id, nameof(TournamentContext)), match_result_data);
        }

        public Event[] Map_person_stats(StoreMatchResultCommandContextModel ctx_model, MatchResultNotificationCommand cmd)
        {
            var home_sets = cmd.Results.Count(set => set == SetResult.Home || set == SetResult.Drawn);
            var away_sets = cmd.Results.Count(set => set == SetResult.Away || set == SetResult.Drawn);

            var match_result = Evaluate_match_result(home_sets, away_sets);

            var home_data = Map(ctx_model.Tournament_id, cmd.MatchId, ctx_model.Home_player_ids, match_result.home);
            var away_data = Map(ctx_model.Tournament_id, cmd.MatchId, ctx_model.Away_player_ids, match_result.away);

            var datas = home_data.Concat(away_data);
            return datas.Select(data => new PersonMatchStatsCreated(nameof(PersonMatchStatsCreated),
                new PersonsContext(data.Person_id, nameof(PersonsContext)), data)).ToArray();
        }

        public IEnumerable<PersonMatchStatsData> Map(string tournament_id, string match_id, IEnumerable<string> player_ids,
                                                     PersonMatchStatsData.Result result)
        {
            return player_ids.Select(player_id => new PersonMatchStatsData()
            {
                Tournament_id = tournament_id,
                Match_id = match_id,
                Person_id = player_id,
                Match_result = result
            });
        }

        public (PersonMatchStatsData.Result home, PersonMatchStatsData.Result away) Evaluate_match_result(int home_sets, int away_sets)
        {
            if (home_sets > away_sets)
                return (PersonMatchStatsData.Result.Won, PersonMatchStatsData.Result.Loose);
            else if (home_sets < away_sets)
                return (PersonMatchStatsData.Result.Loose, PersonMatchStatsData.Result.Won);
            else if (home_sets == away_sets)
                return (PersonMatchStatsData.Result.Drawn, PersonMatchStatsData.Result.Drawn);
            else return (PersonMatchStatsData.Result.None, PersonMatchStatsData.Result.None);
        }
    }
}