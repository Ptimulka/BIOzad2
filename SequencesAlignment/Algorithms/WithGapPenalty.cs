using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencesAlignment.Algorithms
{
    public class WithGapPenalty
    {
        private SimilarityMatrix similarityMatrix;
        private Func<int, double> penaltyFunc;

        public WithGapPenalty(SimilarityMatrix similarityMatrix, Func<int, double> penaltyFunc)
        {
            this.similarityMatrix = similarityMatrix;
            this.penaltyFunc = penaltyFunc;
        }

        /// <summary>
        /// Starts algorithm.
        /// </summary>
        /// <param name="X">First sequence.</param>
        /// <param name="Y">Second sequence.</param>
        /// <returns>Two alignments.</returns>
        public Tuple<double, Tuple<string, string>> StartNoGapAlgorithm(string X, string Y)
        {
            if (X.Length == 0)
            {
                string indels = String.Concat(Enumerable.Repeat("-", Y.Length));
                double evaluation = Penalty(Y.Length);
                return new Tuple<double, Tuple<string, string>>(evaluation, new Tuple<string, string>(indels, Y));
            }
            else if (Y.Length == 0)
            {
                string indels = String.Concat(Enumerable.Repeat("-", X.Length));
                double evaluation = Penalty(X.Length);
                return new Tuple<double, Tuple<string, string>>(evaluation, new Tuple<string, string>(X, indels));
            }


            int tableLenX = X.Length + 1;
            int tableLenY = Y.Length + 1;

            double[,] A = new double[tableLenX, tableLenY];
            double[,] B = new double[tableLenX, tableLenY];
            double[,] C = new double[tableLenX, tableLenY];
            Cell[,] S = new Cell[tableLenX, tableLenY];

            InitTables(A, B, C, S);
            FillTables(A, B, C, S, X, Y);
            return GetAlignments(S, X, Y);
        }

        class Cell
        {
            public Cell(double value, Tuple<int, int> pointer)
            {
                Value = value;
                Pointer = pointer;
            }

            public double Value { get; set; }
            public Tuple<int,int> Pointer { get; set; }
        }

        private void InitTables(double[,] A, double[,] B, double[,] C, Cell[,] S)
        {
            for(int i = 0; i < A.GetLength(0); i++)
            {
                for(int j = 0; j < A.GetLength(1); j++)
                {
                    A[i, j] = double.NegativeInfinity;
                    B[i, j] = double.NegativeInfinity;
                    C[i, j] = double.NegativeInfinity;
                    S[i, j] = new Cell(double.NegativeInfinity, null);
                }
            }
            
            S[0, 0].Value = 0;
            for(int i = 1; i < A.GetLength(0); i++)
            {
                double penalty = Penalty(i);
                S[i, 0].Value = -penalty;
                S[i, 0].Pointer = new Tuple<int, int>(i - 1, 0);
                B[i, 0] = -penalty;
            }

            for (int i = 1; i < A.GetLength(1); i++)
            {
                double penalty = Penalty(i);
                S[0, i].Value = -penalty;
                S[0, i].Pointer = new Tuple<int, int>(0, i - 1);
                A[0, i] = -penalty;
            }
                        
        }

        private void FillTables(double[,] A, double[,] B, double[,] C, Cell[,] S, string X, string Y)
        {
            for(int i = 1; i < A.GetLength(0); i++)
            {
                for(int j = 1; j < A.GetLength(1); j++)
                {
                    C[i, j] = S[i - 1, j - 1].Value + similarityMatrix.GetSimilarity(X[i - 1], Y[j - 1]);
                    FillTableA(A, B, C, i, j);
                    FillTableB(A, B, C, i, j);
                    FillTableS(A, B, C, S, i, j);
                }
            }
        }

        private void FillTableA(double[,] A, double[,] B, double[,] C, int i, int j)
        {
            double max = Double.NegativeInfinity;

            for(int k = 0; k < j; k++)
            {
                double fromB = B[i, k];
                double fromC = C[i, k];
                double penalty = Penalty(j - k);
                double res = Math.Max(fromB, fromC) - penalty;
                if (res > max)
                    max = res;
            }

            A[i, j] = max;
        }

        private void FillTableB(double[,] A, double[,] B, double[,] C, int i, int j)
        {
            double max = Double.NegativeInfinity;

            for (int k = 0; k < i; k++)
            {
                double fromA = A[k, j];
                double fromC = C[k, j];
                double penalty = Penalty(i - k);
                double res = Math.Max(fromA, fromC) - penalty;
                if (res > max)
                    max = res;
            }

            B[i, j] = max;
        }

        private void FillTableS(double[,] A, double[,] B, double[,] C, Cell[,] S, int i, int j)
        {
            double fromA = A[i, j];
            double fromB = B[i, j];
            double fromC = C[i, j];

            if(fromC >= fromA && fromC >= fromB)
            {
                S[i, j] = new Cell(fromC, new Tuple<int, int>(i - 1, j - 1));
            }
            else if(fromA >= fromC && fromA >= fromB)
            {
                S[i, j] = new Cell(fromA, new Tuple<int, int>(i, j - 1));
            }
            else
            {
                S[i, j] = new Cell(fromB, new Tuple<int, int>(i - 1, j));
            }
        }

        private Tuple<double, Tuple<string, string>> GetAlignments(Cell[,] S, string X, string Y)
        {
            string retX = "";
            string retY = "";

            Cell currentCell = S[S.GetLength(0) - 1, S.GetLength(1) - 1];
            double evaluation = currentCell.Value;
            int currenti = S.GetLength(0) - 1;
            int currentj = S.GetLength(1) - 1;

            while(true)
            {
                Tuple<int, int> nextCellPointer = currentCell.Pointer;

                if(nextCellPointer.Item1 == currenti - 1 && nextCellPointer.Item2 == currentj - 1)
                {
                    retX = X[currenti - 1] + retX;
                    retY = Y[currentj - 1] + retY;
                    currenti -= 1;
                    currentj -= 1;
                }
                else if(nextCellPointer.Item1 == currenti - 1)
                {
                    retX = X[currenti - 1] + retX;
                    retY = '-' + retY;
                    currenti -= 1;
                }
                else
                {
                    retX = '-' + retX;
                    retY = Y[currentj - 1] + retY;
                    currentj -= 1;
                }

                if (nextCellPointer.Item1 <= 0 && nextCellPointer.Item2 <= 0)
                    break;

                currentCell = S[nextCellPointer.Item1, nextCellPointer.Item2];
            }

            return new Tuple<double, Tuple<string, string>>(evaluation, new Tuple<string, string>(retX, retY));
        }



        private double Penalty(int len)
        {
            return penaltyFunc(len);
        }
    }
}
