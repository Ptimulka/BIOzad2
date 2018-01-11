using System;
using NUnit.Framework;
using SequencesAlignment;
using System.Collections.Generic;

namespace SequencesAlignmentTests.Utils
{
    [TestFixture]
    public class SimilarityMatrixTest
    {
        [Test]
        public void Test_Should_Throw_Exception_Not_Enough_Lines()
        {
            string matrix = "A G\n2.5 3 5.4\n0 4 -2.3";
            Assert.Throws<NullReferenceException>(() => new SimilarityMatrix(matrix));
        }

        [Test]
        public void Test_Should_Throw_Exception_Not_Enough_In_Line()
        {
            string matrix = "A G\n2.5 3 5.4\n0 4\n 10.5 -3.4 1.9";
            Assert.Throws<Exception>(() => new SimilarityMatrix(matrix));
        }

        [Test]
        public void Test_Should_Throw_Exception_Not_Double()
        {
            string matrix = "A G\n2.5 abc 5.4\n0 4\n 10.5 -3.4 1.9";
            Assert.Throws<FormatException>(() => new SimilarityMatrix(matrix));
        }

        [Test]
        public void Test_Should_Throw_Exception_Double_Letter()
        {
            string matrix = "A G T A\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2";
            Assert.Throws<Exception>(() => new SimilarityMatrix(matrix));
        }

        [Test]
        [TestCaseSource(nameof(_matricesTestCases))]
        public void TestSimilarityMatrix(string matrix, double[,] expected)
        {
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrix);
            Assert.AreEqual(expected, similarityMatrix.Matrix);
        }

        static object[] _matricesTestCases =
        {
            new object[]
            {
                "A G\n2.5 3 5.4\n0 4 -2.3\n 10.5 -3.4 1.9",
                new double[,] { { 2.5, 3, 5.4 }, { 0, 4, -2.3 }, { 10.5, -3.4, 1.9 } }
            }
            ,
            new object[]
            {
                "A G T C\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2",
                new double[,] { { 0, -2, -2, -2, - 2 }, { -2, 2, -1, -1, -1 }, { -2, -1, 2, -1, -1 }, { -2, -1, -1, 2, -1 }, { -2, -1, -1, -1, 2 } }
            }
        };

        private string matrixForTestCheckSequenceCorrectness =
            "A G T C\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2";

        [Test]
        public void TestCheckSequenceCorrectness()
        {            
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrixForTestCheckSequenceCorrectness);
            similarityMatrix.CheckSequenceCorrectness("AGACGTTGC");
        }

        [Test]
        public void Test_SHould_Throw_Exception_Unknown_Letter()
        {
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrixForTestCheckSequenceCorrectness);
            Assert.Throws<KeyNotFoundException>(() => similarityMatrix.CheckSequenceCorrectness("AGAXCGTTGC"));            
        }

        [Test]
        public void Test_SHould_Throw_Exception_Lowercase_Letter()
        {
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrixForTestCheckSequenceCorrectness);
            Assert.Throws<Exception>(() => similarityMatrix.CheckSequenceCorrectness("GCtTc"));
        }
        [Test]
        public void Test_Should_Throw_Exception_Not_Letter()
        {
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrixForTestCheckSequenceCorrectness);
            Assert.Throws<Exception>(() => similarityMatrix.CheckSequenceCorrectness("GCCT3GT"));
        }
    }
}
