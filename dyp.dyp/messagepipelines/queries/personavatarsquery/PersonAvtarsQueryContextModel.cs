using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.queries.personavatarsquery
{
    public class PersonAvtarsQueryContextModel : IMessageContext
    {
        public class PersonAvatar
        {
            public string Person_id;
            public string Avatar;
        }

        public List<PersonAvatar> Avatars = new List<PersonAvatar>();
    }
}