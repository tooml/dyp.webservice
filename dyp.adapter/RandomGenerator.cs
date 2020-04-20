using System;

namespace dyp.adapter
{
    public class RandomGenerator
    {
        private readonly Random _rnd;

        public RandomGenerator() => _rnd = new Random();

        public int Generate_random_integer(int min, int max) => _rnd.Next(min, max);

    }
}