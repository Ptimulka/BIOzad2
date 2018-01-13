using System;
using System.Linq;

namespace SequencesAlignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = args.Where(x => !x.StartsWith("-")).ToArray();
            var options = args.Where(x => x.StartsWith("-")).ToArray();

            // Display help message and quit program.
            if (options.Any(x => x == "--help"))
            {
                Console.WriteLine("Syntax:\n");
                Console.WriteLine("   SequencesAlignment [<matrixFilePath>] [<sequencesFilePath>] [--nogappenalty]");
                return;
            }

            // Resolve path to file with matrix from arguments or ask user for the path.
            string matrixFilePath = arguments.ElementAtOrDefault(0);
            if (matrixFilePath == null)
            {
                Console.WriteLine("Path to file with similarity matrix:");
                matrixFilePath = Console.ReadLine();
            }

            FileUtils fu = new FileUtils();
            
            string matrixFileContent = fu.ReadContentFromFile(matrixFilePath);
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(matrixFileContent);

            // Resolve path to file with sequences from arguments or ask user for two sequences.
            string sequencesFilePath = arguments.ElementAtOrDefault(1);
            string sequence1;
            string sequence2;
            if (sequencesFilePath == null)
            {
                Console.WriteLine("First sequences:");
                sequence1 = Console.ReadLine();

                Console.WriteLine("Second sequences:");
                sequence2 = Console.ReadLine();
            }
            else
            {
                string[] sequences = fu.ReadSequencesFromFile(sequencesFilePath);
                sequence1 = sequences[0];
                sequence2 = sequences[1];
            }

            similarityMatrix.CheckSequenceCorrectness(sequence1);
            similarityMatrix.CheckSequenceCorrectness(sequence2);
            
            // Check whether user provioded the --nogappenalty option.
            bool isOptionB = options.Any(x => x == "--nogappenalty");

            if(isOptionB)
            {
                Hirschberg h = new Hirschberg(similarityMatrix);
                var results = h.StartAlgorithm(sequence1, sequence2);
                double evaluation = similarityMatrix.Evaluate(results.Item1, results.Item2);
                Console.WriteLine(results.Item1);
                Console.WriteLine(results.Item2);
                Console.WriteLine("Evaluation: " + evaluation);
            }

        }
    }
}
