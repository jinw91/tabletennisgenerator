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
            scorer.GenerateMetrics();


            string outputFile = "C:\\output\\sample_scoring_output.csv";
            scorer.WriteOutput(outputFile);
        }
    }
}
