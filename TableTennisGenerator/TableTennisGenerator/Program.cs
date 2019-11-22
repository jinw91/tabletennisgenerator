using System;
using System.Collections.Generic;
using System.IO;

namespace TableTennisGenerator
{
    class Program
    {
        private static string _outputDir = "C:\\output\\tournament\\";
        static void Main(string[] args)
        {
            ParseArgs(args);

            if (string.IsNullOrEmpty(_outputDir) || !Directory.Exists(_outputDir))
            {
                Console.WriteLine("Please enter an output directory: ");
                _outputDir = Console.ReadLine();
            }
            int numTournaments = 100;
            int numRounds = 30;
            string output_dir = "C:\\output\\tournament\\" + $"{numTournaments}_tournaments_{numRounds}_rounds";
            Directory.CreateDirectory(output_dir);
            Random rand = new Random();
            for (int i=0; i<numTournaments; i++)
            {
                int numPlayers = rand.Next(8, 100);
                Tournament tournament = new Tournament(numPlayers, numRounds, 2, output_dir, true);
                tournament.BuildTournament();
            }
        }

        public static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
                    case "-O":
                        _outputDir = args[++i];
                        break;
                }
            }
        }
    }
}
