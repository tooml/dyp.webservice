using dyp.messagehandling;

namespace dyp.contracts.messages.commands.deleteperson
{
    public class DeletePersonCommand : Command
    {
        public string Id { get; set; }
    }
}