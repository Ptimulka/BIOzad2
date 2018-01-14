using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SequencesAlignment;
using SequencesAlignment.Algorithms;

namespace SequencesAlignmentTests.Algorithms
{

    [TestFixture]
    class WithGapPenaltyTest
    {
        [Test]
        [TestCaseSource(nameof(_noGapTestCases))]
        public void TestStartNoGapAlgorithm(string matrix, string X, string Y, Func<int, double> penaltyFunc, double evaluation, string resultX, string resultY)
        {
            var similarityMatrix = new SimilarityMatrix(matrix);

            var withGapPenalty = new WithGapPenalty(similarityMatrix, penaltyFunc);
            var tuple = withGapPenalty.StartNoGapAlgorithm(X, Y);
            Assert.AreEqual(evaluation, tuple.Item1);
            Assert.AreEqual(resultX, tuple.Item2.Item1);
            Assert.AreEqual(resultY, tuple.Item2.Item2);

        }

        static object[] _noGapTestCases =
        {
            new object[]
            {
                "A G T C\n0 0 0 0 0\n0 2 0 0 0\n0 0 2 0 0\n0 0 0 2 01\n0 0 0 0 2",
                "AGTACGCA",
                "TATGC",
                new Func<int, double>(x => x + 1),
                3,
                "AGTACGCA",
                "--TATGC-"
            },
            new object[]
            {   // nagroda wieksza kwadratowo dla dluzszych gap, 
                "A G T C\n0 0 0 0 0\n0 2 0 0 0\n0 0 2 0 0\n0 0 0 2 01\n0 0 0 0 2",
                "AGTACGCA",
                "TATGC",
                new Func<int, double>(x => -2 - x*x),
                93,
                "AGTACGCA-----",
                "--------TATGC"
            },
            new object[]
            {   // nagroda zawsze taka sama - algorytm chce jak najwiecej gap
                "A G T C\n0 0 0 0 0\n0 2 0 0 0\n0 0 2 0 0\n0 0 0 2 01\n0 0 0 0 2",
                "AGTACGCA",
                "TATGC",
                new Func<int, double>(x => -5),
                55,
                "A-G-T-A-C-GCA",
                "-T-A-T-G-C---"
            },
            new object[]
            {   // Z wykladu
                "A G T C\n0 0 0 0 0\n0 2 0 0 0\n0 0 2 0 0\n0 0 0 2 01\n0 0 0 0 2",
                "AGAGTCAATCCATAG",
                "CAGAGGTCCATCATG",
                new Func<int, double>(x => x + 2),
                12,
                "-AGA-GTCAATCCATAG",
                "CAGAGGTCCAT-CAT-G"
            }
        };
    }
}
