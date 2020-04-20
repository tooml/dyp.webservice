using System;
using System.Collections.Generic;
using System.Linq;

namespace dyp.contracts.data
{
    public class RankingDataBasis
    {
        public class RankingOptionsData
        {
            public int Points;
            public int Points_drawn;
            public bool Drawn;
            public bool Walkover;
        }

        public class RankingPlayerData
        {
            public string Player_id;
            public string Player_name;
        }

        public class RankingMatchData
        {
            public class Team
            {
                public string Player_one_id;
                public string Player_two_id;
                public int Sets;
            }

            public Team Home;
            public Team Away;

            public bool Drawn;

            public IEnumerable<string> Get_match_home_players()
            {
                return new string[] { Home.Player_one_id, Home.Player_two_id };
            }

            public IEnumerable<string> Get_match_away_players()
            {
                return new string[] { Away.Player_one_id, Away.Player_two_id };
            }
            public IEnumerable<string> Get_match_players()
            {
                return Get_match_home_players().Concat(Get_match_away_players());
            }

            public IEnumerable<string> Get_match_winners()
            {
                if (Home.Sets > Away.Sets) return Get_match_home_players();
                else if (Home.Sets < Away.Sets) return Get_match_away_players();
                else return Enumerable.Empty<string>();
            }

            public IEnumerable<string> Get_match_loosers()
            {
                if (Home.Sets > Away.Sets) return Get_match_away_players();
                else if (Home.Sets < Away.Sets) return Get_match_home_players();
                else return Enumerable.Empty<string>();
            }

            public IEnumerable<string> Get_match_drawn()
            {
                if (Home.Sets == Away.Sets) return Get_match_players();
                else return Enumerable.Empty<string>();
            }
        }

        public RankingOptionsData Options;
        public List<RankingPlayerData> Players = new List<RankingPlayerData>();
        public List<RankingMatchData> Matches = new List<RankingMatchData>();
        public List<string> Walkover_played = new List<string>();
    }

    public class Ranking
    {
        public class RankingRow
        {
            public string Player_id { get; set; } = string.Empty;
            public string Player_name { get; set; } = string.Empty;
            public int Matches_played { get; set; } = 0;
            public int Wins { get; set; } = 0;
            public int Drawn { get; set; } = 0;
            public int Loose { get; set; } = 0;
            public int Points { get; set; } = 0;
            public decimal Q1 { get => (Points != 0) ? decimal.Divide((decimal)Points, (decimal)Matches_played) : 0; }
            public decimal Q2_total { get; set; } = 0;
            public decimal Q2 { get => (Q2_total != 0) ? decimal.Divide((decimal)Q2_total, (decimal)Matches_played) : 0; }
            public int Walkover { get; set; } = 0;
        }

        public List<RankingRow> Ranking_rows = new List<RankingRow>();

        public Ranking(IEnumerable<RankingDataBasis.RankingPlayerData> players)
        {
            Ranking_rows = players.Select(p =>
            new RankingRow()
            {
                Player_id = p.Player_id,
                Player_name = p.Player_name
            }).ToList();
        }

        public RankingRow Get_row(string player_id)
        {
            return Ranking_rows.FirstOrDefault(row => row.Player_id.Equals(player_id));
        }

        public IEnumerable<RankingRow> Get_rows(IEnumerable<string> player_ids)
        {
            foreach (var player_id in player_ids)
                yield return Get_row(player_id);
        }

        public decimal Get_players_Q1(IEnumerable<string> player_ids)
        {
            return Get_rows(player_ids).Sum(player => player.Q1);
        }

        public void Apply_match_count(IEnumerable<string> player_ids)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => row.Matches_played++);
        }

        public void Apply_match_win(IEnumerable<string> player_ids, int points)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => {
                row.Wins++;
                row.Points += points;
            });
        }

        public void Apply_match_drawn(IEnumerable<string> player_ids, int points)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => {
                row.Drawn++;
                row.Points += points;
            });
        }

        public void Apply_match_loose(IEnumerable<string> player_ids)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => row.Loose++);
        }

        public void Apply_walkover(IEnumerable<string> player_ids)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => row.Walkover++);
        }

        public void Apply_Q2_total(IEnumerable<string> player_ids, decimal value)
        {
            var rows = Get_rows(player_ids);
            Apply(rows, row => row.Q2_total += value);
        }

        private void Apply(IEnumerable<RankingRow> rows, Action<RankingRow> action)
        {
            rows.ToList().ForEach(row => action(row));
        }
    }
}