﻿using NUnit.Framework;
using SequencesAlignment;
using System;

namespace SequencesAlignmentTests.Algorithms
{
    [TestFixture]
    public class HirschbergTest
    {
        [Test]
        [TestCaseSource(nameof(_hirschbergTestCases))]
        public void TestStartAlgorithm(string matrix, string X, string Y, string resultX, string resultY)
        {
            //string matrix = "A G T C\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2";
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrix);

            Hirschberg hirschberg = new Hirschberg(similarityMatrix);
            Tuple<string,string> tuple = hirschberg.StartAlgorithm(X, Y);
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
            },
            new object[]
            {
                "A G T C\n0 -2 -2 -2 -2\n-2 2 -1 -1 -1\n-2 -1 2 -1 -1\n-2 -1 -1 2 -1\n-2 -1 -1 -1 2",
                "AGA",
                "G",
                "AGA",
                "-G-"
            },
            new object[]
            {
                "A G T C\n0 0 0 0 0\n0 1 -1 -1 -1\n0 -1 1 -1 -1\n0 -1 -1 1 -1\n0 -1 -1 -1 1",
                "ATAAGC",
                "AAAAACG",
                "ATAAG--C-",
                "A-AA-AACG"
            },
            new object[]
            {
                "A G T C\n0 -1 -1 -1 -1\n-1 1 0 0 0\n-1 0 1 0 0\n-1 0 0 1 0\n-1 0 0 0 1",
                "ATAAGC",
                "AAAAACG",
                "ATAAGC-",
                "AAAAACG"
            },
            new object[]
            {
                //"A G T C\n0 0 0 0 0\n0 1 0 0 0\n0 0 1 0 0\n0 0 0 1 0\n0 0 0 0 1",
                "A G T C\n0 -1 -1 -1 -1\n-1 1 0 0 0\n-1 0 1 0 0\n-1 0 0 1 0\n-1 0 0 0 1",
                "CTG",
                "CCCG",
                "CT-G",
                "CCCG"
            }
        };


    }
}
