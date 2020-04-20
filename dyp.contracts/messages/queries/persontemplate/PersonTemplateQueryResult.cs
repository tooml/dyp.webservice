using dyp.messagehandling;

namespace dyp.contracts.messages.queries.persontemplate
{
    public class PersonTemplateQueryResult : QueryResult
    {
        public string Id;
        public string FirstName;
        public string LastName;
        public string Image;
    }
}