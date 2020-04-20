using dyp.adapter;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.domain
{
    public static class ListShuffle
    {
        private static readonly RandomGenerator _random_generator = new RandomGenerator();

        public static IEnumerable<T> Shuffle_list<T>(T[] list)
        {
            T temp;

            for (int i = 0; i < list.Count(); i++)
            {
                var rnd_number = _random_generator.Generate_random_integer(i, list.Count());
                temp = list[i];
                list[i] = list[rnd_number];
                list[rnd_number] = temp;
            }

            return list;
        }
    }
}