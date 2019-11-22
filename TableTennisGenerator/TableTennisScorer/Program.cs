using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TableTennisScorer
{
    class Program
    {
        private static string _input = "";
        private static string _output = "";
        public static void Main(string[] args)
        {
            ParseArgs(args);

            while (string.IsNullOrEmpty(_input) && !File.Exists(_input))
            {
                Console.WriteLine("Please enter an input file: ");
                _input = Console.ReadLine();
            }

            while (string.IsNullOrEmpty(_output) && !File.Exists(_output))
            {
                Console.WriteLine("Please enter an output file: ");
                _output = Console.ReadLine();
            }

            Scorer scorer = new Scorer(_input);
            scorer.GenerateMetrics();

            scorer.WriteOutput(_output);
        }

        public static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
                    case "-I":
                        _input = args[++i];
                        break;
                    case "-O":
                        _output = args[++i];
                        break;
                }
            }
        }
    }
}
