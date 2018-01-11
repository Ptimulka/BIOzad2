using System;
using System.Collections.Generic;
using System.Linq;

namespace SequencesAlignment
{
    public class Hirschberg
    {
        public Hirschberg(SimilarityMatrix similarityMatrix)
        {
            this.similarityMatrix = similarityMatrix;
        }

        /// <summary>
        /// Starts Hirschberg algorithm.
        /// </summary>
        /// <param name="X">First sequence.</param>
        /// <param name="Y">Second sequence.</param>
        /// <returns>Two alignments.</returns>
        public Tuple<string, string> StartAlgorithm(string X, string Y)
        {
            Tuple<string, string> t1;
            Tuple<string, string> t2;

            if(X.Length == 0)
            {
                string indels = String.Concat(Enumerable.Repeat("-", Y.Length));
                return new Tuple<string, string>(indels, Y);
            }
            else if(Y.Length == 0)
            {
                string indels = String.Concat(Enumerable.Repeat("-", X.Length));
                return new Tuple<string, string>(X, indels);
            }
            else if (X.Length == 1)
            {
                var t = StartAlgorithmForOneOfLen1(Y, X[0]);
                return new Tuple<string, string>(t.Item2, t.Item1);
            }
            else if (Y.Length == 1)
            {
                return StartAlgorithmForOneOfLen1(X, Y[0]);
            }
            else
            {             
                string halfUpper = X.Substring(0, X.Length / 2);
                string halfBottom = X.Substring(X.Length / 2);

                double[] halfUpperResult = GetLastRow(halfUpper, Y);
                double[] halfBottomResult = GetLastRow(ReverseString(halfBottom), ReverseString(Y));
                Array.Reverse(halfBottomResult);
                var added = halfUpperResult.Zip(halfBottomResult, (x, y) => x + y);
                int indexOfMax = FindMaxIndex(added);

                string partLeft = Y.Substring(0, indexOfMax);
                string partRight = Y.Substring(indexOfMax);

                t1 = StartAlgorithm(halfUpper, partLeft);
                t2 = StartAlgorithm(halfBottom, partRight);
            }

            Tuple<string, string> ret = new Tuple<string, string>(t1.Item1 + t2.Item1, t1.Item2 + t2.Item2);
            return ret;
        }
        

        private SimilarityMatrix similarityMatrix;

        private double[] GetLastRow(string X, string Y)
        {
            double[] row1 = new double[Y.Length + 1];
            double[] row2 = new double[Y.Length + 1];

            row1[0] = 0;
            for(int j = 1; j < row1.Length; j++)
            {
                row1[j] = row1[j - 1] + similarityMatrix.GetSimilarity('-', Y[j - 1]);
            }

            for(int i = 1; i < X.Length+1; i++)
            {
                row2[0] = row1[0] + similarityMatrix.GetSimilarity('-', X[i - 1]);

                for(int j = 1; j < row1.Length; j++)
                {
                    double scoreDown = row1[j] + similarityMatrix.GetSimilarity('-', X[i - 1]);
                    double scoreRight = row2[j - 1] + similarityMatrix.GetSimilarity('-', Y[j - 1]);
                    double scoreDiag = row1[j - 1] + similarityMatrix.GetSimilarity(X[i - 1], Y[j - 1]);

                    double result = Math.Max(Math.Max(scoreDiag, scoreRight), scoreDown);
                    row2[j] = result;
                }
                
                double[] tmp = row1;
                row1 = row2;
                row2 = tmp;           
            }

            return row1;
        }

        private string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private int FindMaxIndex(IEnumerable<double> values)
        {
            int index = -1;
            double maxValue = double.MinValue;
            for(int i = 0; i < values.Count() ; i++)
            {
                if(values.ElementAt(i) > maxValue)
                {
                    index = i;
                    maxValue = values.ElementAt(i);
                }
            }
            return index;
        }

        private Tuple<string, string> StartAlgorithmForOneOfLen1(string longer, char letter)
        {
            double upperLeft = 0;
            double upperRight = similarityMatrix.GetSimilarity('-', letter);

            int lastFromLeftOrDiagIndex = 0;
            bool lastFromLeft = true;
            for(int i = 1; i < longer.Length+1; i++)
            {
                double bottomLeft = upperLeft + similarityMatrix.GetSimilarity('-', longer[i-1]);

                double scoreDown = upperRight + similarityMatrix.GetSimilarity('-', longer[i-1]);
                double scoreRight = bottomLeft + similarityMatrix.GetSimilarity('-', letter);
                double scoreDiag = upperLeft + similarityMatrix.GetSimilarity(longer[i-1], letter);
                double bottomRight;

                if(scoreDiag >= scoreDown && scoreDiag >= scoreRight)
                {
                    lastFromLeftOrDiagIndex = i;
                    lastFromLeft = false;
                    bottomRight = scoreDiag;
                }
                else if(scoreRight >= scoreDiag && scoreRight >= scoreDown)
                {
                    lastFromLeftOrDiagIndex = i;
                    lastFromLeft = true;
                    bottomRight = scoreRight;
                }
                else
                {
                    bottomRight = scoreDown;
                }

                upperLeft = bottomLeft;
                upperRight = bottomRight;
            }
            
            return GetAlignmentsForOneOfLen1(lastFromLeftOrDiagIndex, lastFromLeft, longer, letter);
        }
        
        private Tuple<string, string> GetAlignmentsForOneOfLen1(int lastFromLeftOrDiagIndex, bool lastFromLeft, string longer, char letter)
        {
            string ret1 = "";
            string ret2 = "";

            if (lastFromLeft)
            {
                for (int i = 0; i < lastFromLeftOrDiagIndex; i++)
                {
                    ret1 += longer[i];
                    ret2 += "-";
                }
                ret1 += "-";
                ret2 += letter;

                for (int i = lastFromLeftOrDiagIndex; i < longer.Length; i++)
                {
                    ret1 += longer[i];
                    ret2 += "-";
                }
            }
            else
            {
                for (int i = 0; i < lastFromLeftOrDiagIndex - 1; i++)
                {
                    ret1 += longer[i];
                    ret2 += "-";
                }
                ret1 += longer[lastFromLeftOrDiagIndex - 1];
                ret2 += letter;

                for (int i = lastFromLeftOrDiagIndex; i < longer.Length; i++)
                {
                    ret1 += longer[i];
                    ret2 += "-";
                }
            }

            return new Tuple<string, string>(ret1, ret2);
        }
    }
}
