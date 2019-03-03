using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodokuLib
{
    public static class SudokuUtil
    {
        private static Random random = new Random();

        public static int[] GetPermutation(int max)
        {
            var values = Enumerable.Range(1, max).ToList();
            var permutation = new List<int>();
            for (int i = 0; i < max; i++)
            {
                int randIndex = random.Next(values.Count());
                permutation.Add(values[randIndex]);
                values.RemoveAt(randIndex);
            }

            return permutation.ToArray();
        }
    }
}
