using dyp.contracts.data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.domain
{
    public class SimpleTeamBuilder
    {
        public IEnumerable<Team> Determine_teams(IEnumerable<Player> players)
        {
            var shuffled_competitors = ListShuffle.Shuffle_list(players.ToArray());
            var pairs = ListPairing.Pairing_list(shuffled_competitors);

            return Build_teams(pairs);
        }

        private IEnumerable<Team> Build_teams(IEnumerable<Tuple<Player, Player>> competitior_pairs)
        {
            return competitior_pairs.Select(pair =>
                                            new Team() { Member_one = pair.Item1, Member_two = pair.Item2 });
        }
    }
}