using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SequencesAlignment
{
    public class SimilarityMatrix
    {
        /// <summary>
        /// Creates new SimilarityMatrix instance
        /// File with matrix should contain:
        /// in the first line - list of letters divided by whitespace,
        /// n+1 lines, each with n+1 double values where n is number of letters,
        /// +1 is because of '-', supposed to be first 'letter' (on position 0 in matrix),
        /// double value in i-th line, on j-th position is similarity of i and j letters written in first line of file,
        /// Example:
        /// A G T C
        /// 0 -2 -2 -2 -2
        /// -2 2 -1 -1 -1
        /// -2 -1 2 -1 -1
        /// -2 -1 -1 2 -1
        /// -2 -1 -1 -1 2
        /// </summary>
        /// <param name="fileContent">String with similarity matrix. </param>
        public SimilarityMatrix(string fileContent)
        {
            using (StringReader sr = new StringReader(fileContent))
            {
                indices = new Dictionary<char, int>();
                FillIndices(sr);
                int len = indices.Keys.Count;
                Matrix = new double[len,len];
                FillMatrix(sr);
            }
        }

        /// <summary>
        /// Gets similarity of two given letters.
        /// </summary>
        /// <param name="letter1">First letter.</param>
        /// <param name="letter2">Second letter.</param>
        /// <returns>Similarity of given letters.</returns>
        public double GetSimilarity(char letter1, char letter2)
        {
            int index1 = indices[letter1];
            int index2 = indices[letter2];
            return Matrix[index1, index2];
        }

        /// <summary>
        /// Checks whether given sequence is correct.
        /// If sequence contains chars which are not letters or lowercase letter,
        /// or given letters are not in similarity matrix, exception is thrown.
        /// </summary>
        /// <param name="sequence">Sequence which correctness should be checked.</param>
        public void CheckSequenceCorrectness(string sequence)
        {
            for(int i = 0; i < sequence.Length; i++)
            {
                char letter = sequence[i];
                if (!char.IsLetter(letter) || char.IsLower(letter))
                    throw new Exception(letter + " is not a lettter or is lowercase!");
                int index = indices[sequence[i]];
            }
        }


        private Dictionary<char, int> indices;
        public double[,] Matrix { get; set; }

        private bool IsUppercaseLetter(string str)
        {
            return (str.Length == 1 && char.IsLetter(str[0]) && !char.IsLower(str[0]));
        }

        private void FillIndices(StringReader sr)
        {
            string[] letters = sr.ReadLine().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            if (letters.Length < 1)
            {
                throw new Exception("Not enought letters (min 1)");
            }

            Array.ForEach(letters, element =>
            {
                if (!IsUppercaseLetter(element))
                    throw new Exception(element + " is not a letter!");
            });
            
            indices['-'] = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                if (indices.ContainsKey(letters[i][0]))
                    throw new Exception("Letter found twice: " + letters[i][0]);
                indices.Add(letters[i][0], i + 1);
            }
        }

        private void FillMatrix(StringReader sr)
        {
            int len = Matrix.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                string[] values = sr.ReadLine().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                if(values.Length != len)
                {
                    throw new Exception("Not enough values in matrix - remember about '-'!");
                }
                for(int j = 0; j < len; j++)
                {
                    Matrix[i, j] = double.Parse(values[j], CultureInfo.InvariantCulture);
                }
            }
        }

    }
}
