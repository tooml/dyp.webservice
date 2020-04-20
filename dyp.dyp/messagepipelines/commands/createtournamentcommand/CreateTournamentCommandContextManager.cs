using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.commands.createtournamentcommand
{
    public class CreateTournamentCommandContextManager : PersonsContextManager<CreateTournamentCommandContextModel>
    {
        public CreateTournamentCommandContextManager(IEventStore es) : base(es) { }
    }
}