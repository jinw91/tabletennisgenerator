using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TableTennisGenerator
{
    class Program
    {
        private static string _outputDir = "C:\\output\\tournament\\";
        private static string _inputFile = "";
        private static string _numRoundsInput = "";
        private static string _numSimultaneousMatches = "";
        static void Main(string[] args)
        {
            ParseArgs(args);
            while (string.IsNullOrEmpty(_inputFile) || !File.Exists(_inputFile))
            {
                Console.WriteLine("Please enter an input file: ");
                _inputFile = Console.ReadLine();
                if (!File.Exists(_inputFile))
                {
                    Console.WriteLine("File does not exist");
                }
            }

            int numRounds;
            while (!int.TryParse(_numRoundsInput, out numRounds))
            {
                Console.WriteLine("Please enter a valid number of rounds to play: ");
                _numRoundsInput = Console.ReadLine();
            }

            int numSimultaneousMatches;
            while (!int.TryParse(_numSimultaneousMatches, out numSimultaneousMatches))
            {
                Console.WriteLine("Please enter a valid number of simultaneous matches allowed per round: ");
                _numSimultaneousMatches = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(_outputDir) || !Directory.Exists(_outputDir))
            {
                if (!Directory.Exists(_outputDir))
                {
                    Console.WriteLine("Directory does not exist, please make it before setting. ");
                }
                Console.WriteLine("Please enter an output directory: ");
                _outputDir = Console.ReadLine();
            }

            Tournament tournament = new Tournament(_inputFile, numRounds, numSimultaneousMatches, _outputDir);
            tournament.BuildTournament();
        }

        public static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
                    case "-I":
                        _inputFile = args[++i];
                        break;

                    case "-O":
                        _outputDir = args[++i];
                        break;

                    case "-R":
                        _numRoundsInput = args[++i];
                        // Leaving numRounds to be string so we can re-enter input if corrupt rather than throw exception at beginning
                        break;

                    case "-S":
                        _numSimultaneousMatches = args[++i];
                        // Leaving numSimultaneousMatches to be string so we can re-enter input if corrupt rather than throw exception at beginning
                        break;
                }
            }
        }
    }
}
