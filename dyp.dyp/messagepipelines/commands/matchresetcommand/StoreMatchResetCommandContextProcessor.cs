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

namespace dyp.dyp.messagepipelines.commands.matchresetcommand
{
    public class StoreMatchResetCommandContextProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var ctx_model = model as StoreMatchResetCommandContextModel;
            var match_reset_event = Map_match_reset(ctx_model);
            var person_stats_events = Map_person_stats(ctx_model);

            var events = new List<Event>(person_stats_events);
            events.Add(match_reset_event);

            return new CommandOutput(new Success(), events.ToArray());
        }

        private Event Map_match_reset(StoreMatchResetCommandContextModel ctx_model)
        {
            var match_reset_data = new MatchResetData()
            {
                Tournament_id = ctx_model.Tournament_id,
                Match_id = ctx_model.Match_id
            };

            return new MatchReseted(nameof(MatchReseted),
                new TournamentContext(ctx_model.Tournament_id, nameof(TournamentContext)), match_reset_data);
        }

        public Event[] Map_person_stats(StoreMatchResetCommandContextModel ctx_model)
        {
            var datas = ctx_model.Player_ids.Select(player_id => Map(ctx_model.Tournament_id,
                ctx_model.Match_id,
                player_id,
                PersonMatchStatsData.Result.None));

            return datas.Select(data => new PersonMatchStatsDeleted(nameof(PersonMatchStatsDeleted),
                new PersonsContext(data.Person_id, nameof(PersonsContext)), data)).ToArray();
        }

        public PersonMatchStatsData Map(string tournament_id, string match_id, string player_id,
                                                     PersonMatchStatsData.Result result)
        {
            return new PersonMatchStatsData()
            {
                Tournament_id = tournament_id,
                Match_id = match_id,
                Person_id = player_id,
                Match_result = result
            };
        }
    }
}