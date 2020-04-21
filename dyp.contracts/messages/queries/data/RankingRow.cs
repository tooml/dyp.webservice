namespace dyp.contracts.messages.queries.data
{
    public class RankingRow
    {
        public int Rank { get; set; }
        public string PlayerName { get; set; }
        public int Matches { get; set; }
        public int W { get; set; }
        public int D { get; set; }
        public int L { get; set; }
        public int Points { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
    }
}