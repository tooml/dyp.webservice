namespace dyp.contracts.data
{
    public class Team
    {
        public Player Member_one { get; set; }
        public Player Member_two { get; set; }
        public double Strength => ((Member_one.Strength + Member_two.Strength) / 2);
    }
}