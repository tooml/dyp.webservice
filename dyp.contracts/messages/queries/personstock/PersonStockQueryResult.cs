using dyp.contracts.messages.queries.data;
using dyp.messagehandling;

namespace dyp.contracts.messages.queries.personstock
{
    public class PersonStockQueryResult : QueryResult
    {
        public Person[] Persons { get; set; }
    }
}