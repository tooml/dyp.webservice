using dyp.messagehandling.pipeline.messagecontext;
using System.Collections.Generic;

namespace dyp.dyp.messagepipelines.commands.changeplayerscommand
{
    public class ChangePlayersCommandContextModel : IMessageContext
    {
        public class Person
        {
            public string Id;
            public string First_name;
            public string Last_name;
            public string Image;
        }

        public class Player
        {
            public string Id;
            public bool Enabled;
        }

        public List<Person> Persons = new List<Person>();
        public List<Player> Tournament_Players = new List<Player>();
    }
}