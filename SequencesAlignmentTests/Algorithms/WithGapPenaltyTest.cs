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
        [TestCaseSource(nameof(_hirschbergTestCases))]
        public void TestStartAlgorithm(string matrix, string X, string Y, string resultX, string resultY)
        {
            var similarityMatrix = new SimilarityMatrix(matrix);

            var withGapPenalty = new WithGapPenalty(similarityMatrix, x => x + 1);
            var tuple = withGapPenalty.StartAlgorithm(X, Y);
            Assert.AreEqual(resultX, tuple.Item1);
            Assert.AreEqual(resultY, tuple.Item2);

        }

        static object[] _hirschbergTestCases =
        {
            new object[]
            {
                "A G T C\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2",
                "AGTACGCA",
                "TATGC",
                "AGTACGCA",
                "--TATGC-"
            }
        };
    }
}
