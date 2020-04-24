using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines
{
    public class PersonsContextModel : IMessageContext
    {
        public class PersonInfo
        {
            public string Id;
            public string First_name;
            public string Last_name;
        }

        public IEnumerable<PersonInfo> Persons;
    }
}