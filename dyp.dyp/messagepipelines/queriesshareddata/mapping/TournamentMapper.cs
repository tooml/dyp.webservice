using System.Linq;
using static dyp.dyp.messagepipelines.queriesshareddata.TournamentData;

namespace dyp.dyp.messagepipelines.mapping
{
    public static class TournamentMapper
    {
        public static contracts.messages.queries.data.Round Map(Round round, string tournament_id)
        {
            return new contracts.messages.queries.data.Round()
            {
                Id = round.Id,
                Name = Round_name(round.Count),
                TournamentId = tournament_id,
                Matches = round.Matches.Select(m => Map(m))
            };
        }

        public static contracts.messages.queries.data.Match Map(Match match)
        {
            return new contracts.messages.queries.data.Match()
            {
                Id = match.Id,
                Table = match.Table,
                Sets = match.Sets,
                Drawn = match.Drawn,
                Home = Map(match.Home),
                Away = Map(match.Away),
                SetResults = match.Results.Select(result => Map(result)).ToList()
            };
        }

        public static contracts.messages.queries.data.Team Map(Team team)
        {
            return new contracts.messages.queries.data.Team()
            {
                PlayerOne = Map(team.Player_one),
                PlayerTwo = Map(team.Player_two)
            };
        }

        public static contracts.messages.queries.data.Player Map(Player player)
        {
            return new contracts.messages.queries.data.Player()
            {
                Id = player.Id,
                FirstName = player.First_name,
                LastName = player.Last_name,
                FullName = string.Concat(player.First_name, ", ", player.Last_name),
                FullNameShort = string.Concat(player.First_name, ", ", player.Last_name.First(), "."),
                Image = player.Image
            };
        }

        public static contracts.messages.queries.data.SetResult Map(SetResult result)
        {
            return (contracts.messages.queries.data.SetResult)result;
        }

        public static string Round_name(int rounds_played)
        {
            return string.Concat("Round ", rounds_played + 1);
        }
    }
}