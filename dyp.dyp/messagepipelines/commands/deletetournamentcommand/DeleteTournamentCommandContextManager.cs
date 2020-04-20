using dyp.contracts.messages.commands.deletetournamentcommand;
using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.deletetournamentcommand
{
    public class DeleteTournamentCommandContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;

        public DeleteTournamentCommandContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            var cmd = input as DeleteTournamentCommand;
            var tournament_exist = _es.Replay(typeof(TournamentCreated)).Any(record =>
            {
                var tournamen_data = record.Data as TournamentData;
                return tournamen_data.Id.Equals(cmd.Id);
            });

            return new DeleteTournamentCommandContextModel() { Tournament_existing = tournament_exist };
        }

        public void Update(IEnumerable<Event> events) { }
    }
}