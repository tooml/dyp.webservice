﻿namespace dyp.contracts.data
{
    public class Player
    {
        public string Id;
        public string First_name;
        public string Last_name;
        public int Matches;
        public int Walkover_played;
        public int Strength_amount;
        public double Strength => ((double)Strength_amount / Matches);
    }
}