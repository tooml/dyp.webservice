using dyp.messagehandling;
using System.Collections.Generic;

namespace dyp.contracts.messages.commands.matchresult
{
    public enum SetResult
    {
        Home,
        Away,
        Drawn,
        None
    }

    public class MatchResultNotificationCommand : Command
    {
        public string MatchId;
        public IEnumerable<SetResult> Results;
    }
}