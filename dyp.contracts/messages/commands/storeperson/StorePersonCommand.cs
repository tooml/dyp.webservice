using dyp.messagehandling;

namespace dyp.contracts.messages.commands.storeperson
{
    public class StorePersonCommand : Command
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
    }
}