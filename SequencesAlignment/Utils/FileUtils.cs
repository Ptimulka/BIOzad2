using System;
using System.IO;

namespace SequencesAlignment
{
    class FileUtils
    {
        /// <summary>
        /// Reads sequences from file. They must be divided by whitespace. If number of sequences != 2 or cannot read the file, exception is thrown. 
        /// The correctness of seqences is NOT checked.
        /// </summary>
        /// <param name="filePath">Path to file with sequences.</param>
        /// <returns>Array with two elements - first and second sequence.</returns>
        public string[] ReadSequencesFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {              
                string line = sr.ReadToEnd();
                string[] sequences = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                if(sequences.Length < 2)
                {
                    throw new Exception("Sequences file contains less that two sequences!");
                }

                return sequences;
            }
        }

        /// <summary>
        /// Reads content of file. If it is not possible, exception is thrown.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>Content of the file.</returns>
        public string ReadContentFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();               
            }
        }
    }
}
