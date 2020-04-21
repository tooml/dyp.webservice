using System.Collections.Generic;

namespace dyp.contracts.messages.queries.data
{
    public class Tournament
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Created { get; set; }

        public Options Options { get; set; }

        public IEnumerable<Round> Rounds { get; set; }
    }
}