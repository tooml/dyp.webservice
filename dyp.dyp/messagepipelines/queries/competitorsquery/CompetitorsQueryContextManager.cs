
using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.queries.competitorsquery
{
    public class CompetitorsQueryContextManager : PersonsContextManager<CompetitorsQueryContextModel>
    {
        public CompetitorsQueryContextManager(IEventStore es) : base(es) { }
    }
}