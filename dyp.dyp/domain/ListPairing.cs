using System;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.domain
{
    public static class ListPairing
    {
        public static IEnumerable<Tuple<T, T>> Pairing_list<T>(IEnumerable<T> list)
        {
            var paring_list = new List<Tuple<T, T>>();

            for (int i = 0; i < list.Count(); i += 2)
            {
                var pair = new Tuple<T, T>(list.ElementAt(i), list.ElementAt(i + 1));
                paring_list.Add(pair);
            }

            return paring_list;
        }
    }
}