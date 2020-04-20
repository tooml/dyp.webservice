using dyp.provider.eventstore;

namespace dyp.dyp.messagepipelines.queries.personsstockquery
{
    public class PersonStockQueryContextManager : PersonsContextManager<PersonStockQueryContextModel>
    {
        public PersonStockQueryContextManager(IEventStore es) : base(es) { }
    }
}