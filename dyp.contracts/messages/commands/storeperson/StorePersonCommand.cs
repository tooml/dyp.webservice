using dyp.messagehandling;

namespace dyp.contracts.messages.commands.storeperson
{
    public class StorePersonCommand : Command
    {
        public string Id;
        public string FirstName;
        public string LastName;
        public string Image;
    }
}