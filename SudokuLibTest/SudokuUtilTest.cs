using Microsoft.VisualStudio.TestTools.UnitTesting;
using SodokuLib;
using System;
using System.Linq;

namespace SudokuLibTest
{
    [TestClass]
    public class SudokuUtilTest
    {
        [TestMethod]
        public void GetPermutationTest1()
        {
            int max = 9;
            int[] values = SudokuUtil.GetPermutation(max);
            for (int i = 1; i <= max; i++)
            {
                Assert.IsTrue(values.Contains(i), $"The permutation does not container {i}");
            }
        }
    }
}
