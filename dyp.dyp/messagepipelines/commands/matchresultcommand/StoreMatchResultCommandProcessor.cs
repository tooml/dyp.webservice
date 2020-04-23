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

            var sets = Count_sets(cmd.Results);
            var match_result = Evaluate_match_result(sets);


            var match_result_ev = Map_match_result(ctx_model, cmd);
            var person_stats_ev = Map_person_stats(ctx_model, match_result);
            var player_strength_ev = Map_person_strength(sets, ctx_model);

            var events = new List<Event>(person_stats_ev);
            events.Add(match_result_ev);
            events.AddRange(player_strength_ev);

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

        public Event[] Map_person_stats(StoreMatchResultCommandContextModel ctx_model,
                                        (PersonMatchStatsData.Result home, PersonMatchStatsData.Result away) match_result)
        {
            var home_data = Map(ctx_model.Tournament_id, ctx_model.Match_id, ctx_model.Home_player, match_result.home);
            var away_data = Map(ctx_model.Tournament_id, ctx_model.Match_id, ctx_model.Away_player, match_result.away);

            var datas = home_data.Concat(away_data);
            return datas.Select(data => new PersonMatchStatsCreated(nameof(PersonMatchStatsCreated),
                new PersonsContext(data.Person_id, nameof(PersonsContext)), data)).ToArray();
        }

        public Event[] Map_person_strength((int Home_sets, int Away_sets) sets, StoreMatchResultCommandContextModel ctx_model)
        {
            var home_players_strength = sets.Home_sets - (ctx_model.Home_strength / 2);
            var away_players_strength = sets.Away_sets - (ctx_model.Away_strength / 2);

            var home_players_ev = Map(ctx_model.Home_player, ctx_model, home_players_strength);
            var away_players_ev = Map(ctx_model.Away_player, ctx_model, away_players_strength);

            return home_players_ev.Concat(away_players_ev).ToArray();
        }

        private IEnumerable<Event> Map(IEnumerable<string> players, 
                                       StoreMatchResultCommandContextModel ctx_model, int strength)
        {
            return players.Select(id => new PlayerStrengthChanged(nameof(PlayerStrengthChanged),
                new TournamentContext(ctx_model.Tournament_id, nameof(TournamentContext)), new PlayerStrengthData()
                {
                    Match_id = ctx_model.Match_id,
                    Player_id = id,
                    Strength_amount = strength
                }));
        }

        public IEnumerable<PersonMatchStatsData> Map(string tournament_id, string match_id,
                                                     IEnumerable<string> players,
                                                     PersonMatchStatsData.Result result)
        {
            return players.Select(id => new PersonMatchStatsData()
            {
                Tournament_id = tournament_id,
                Match_id = match_id,
                Person_id = id,
                Match_result = result
            });
        }

        private (int Home_sets, int Away_sets) Count_sets(IEnumerable<SetResult> results)
        {
            var home_sets = results.Count(set => set == SetResult.Home || set == SetResult.Drawn);
            var away_sets = results.Count(set => set == SetResult.Away || set == SetResult.Drawn);

            return (home_sets, away_sets);
        }

        private (PersonMatchStatsData.Result home, PersonMatchStatsData.Result away) Evaluate_match_result((int Home_sets, int Away_sets) sets)
        {
            if (sets.Home_sets > sets.Away_sets)
                return (PersonMatchStatsData.Result.Won, PersonMatchStatsData.Result.Loose);
            else if (sets.Home_sets < sets.Away_sets)
                return (PersonMatchStatsData.Result.Loose, PersonMatchStatsData.Result.Won);
            else if (sets.Home_sets == sets.Away_sets)
                return (PersonMatchStatsData.Result.Drawn, PersonMatchStatsData.Result.Drawn);
            else return (PersonMatchStatsData.Result.None, PersonMatchStatsData.Result.None);
        }
    }
}