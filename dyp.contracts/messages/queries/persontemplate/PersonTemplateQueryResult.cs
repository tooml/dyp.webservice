using dyp.messagehandling;

namespace dyp.contracts.messages.queries.persontemplate
{
    public class PersonTemplateQueryResult : QueryResult
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
    }
}