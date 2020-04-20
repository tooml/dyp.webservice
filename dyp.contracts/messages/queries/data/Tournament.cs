using System.Collections.Generic;

namespace dyp.contracts.messages.queries.data
{
    public class Tournament
    {
        public string Id;
        public string Name;
        public string Created;

        public Options Options;

        public IEnumerable<Round> Rounds;
    }
}