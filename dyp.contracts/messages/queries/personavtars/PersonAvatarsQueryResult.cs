using dyp.contracts.messages.queries.data;
using dyp.messagehandling;
using System.Collections.Generic;

namespace dyp.contracts.messages.queries.personavtars
{
    public class PersonAvatarsQueryResult : QueryResult
    {
        public List<PersonAvatar> Avatars { get; set; } = new List<PersonAvatar>();
    }
}