using System;
using System.Collections.Generic;
using System.IO;

namespace TableTennisScorer
{
    class Program
    {
        public static void Main(string[] args)
        {
            string inputFile = "C:\\input\\sample_scoring_input.csv";
            List<Tuple<string, string, string, string, int, int>> matches = ParseCSV(inputFile);

            foreach (Tuple<string, string, string, string, int, int> match in matches)
            {
                Console.WriteLine($"{match.Item1}+{match.Item2} vs {match.Item3}+{match.Item4} score={match.Item5} to {match.Item6}" );
            }
            Console.WriteLine("Hello World!");
        }

        public static List<Tuple<string, string, string, string, int, int>> ParseCSV(string path)
        {
            List<Tuple<string, string, string, string, int, int>> results = new List<Tuple<string, string, string, string, int, int>>();
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(",");


                    if (parts.Length == 8 && Int32.TryParse(parts[6], out int firstRes))
                    {
                        results.Add(new Tuple<string, string, string, string, int, int>(parts[2], parts[3], parts[4], parts[5], Int32.Parse(parts[6]), Int32.Parse(parts[7])));

                    }
                }
            }

            return results;
        }
    }
}
