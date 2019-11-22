using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TableTennisScorer
{
    class Program
    {
        public static void Main(string[] args)
        {
            string inputFile = "C:\\input\\sample_scoring_input.csv";
            Scorer scorer = new Scorer(inputFile);
            Dictionary<string, Dictionary<string, double>> metrics = scorer.GenerateMetrics();


            string outputFile = "C:\\output\\sample_scoring_output.csv";
            using (StreamWriter streamWriter = new StreamWriter(outputFile, false))
            {
                streamWriter.WriteLine("Player,Games Played, Games Won, Points Scored, Points Lost, Points Per Game, Point Differential");
                foreach (KeyValuePair<string, Dictionary<string, double>> keyValuePair in metrics)
                {
                    streamWriter.Write($"{keyValuePair.Key},");
                    foreach (KeyValuePair<string, double> propertyValuePair in keyValuePair.Value)
                    {
                        streamWriter.Write($"{propertyValuePair.Value},");
                    }
                    streamWriter.Write("\n");
                }
            }
        }
    }
}
