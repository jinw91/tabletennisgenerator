using System;
using System.IO;
using TableTennisGenerator;

namespace TestTournamentBuilds
{
    class Program
    {
        private static string _numPlayersInput;
        private static string _outputDir;
        private static string _numRoundsInput = "";
        private static string _numSimultaneousMatches = "";
        private static string _numTournaments = "";
        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(_outputDir) || !Directory.Exists(_outputDir))
            {
                if (!Directory.Exists(_outputDir))
                {
                    Console.WriteLine("Directory does not exist, please make it before setting. ");
                }
                Console.WriteLine("Please enter an output directory: ");
                _outputDir = Console.ReadLine();
            }

            int numTournaments;
            while (!int.TryParse(_numTournaments, out numTournaments))
            {
                Console.WriteLine("Please enter a valid number of rounds to play: ");
                _numTournaments = Console.ReadLine();
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

            string output_dir = Path.Combine(_outputDir, $"{numTournaments}_tournaments_{numRounds}_rounds");
            Directory.CreateDirectory(output_dir);
            Random rand = new Random();
            int.TryParse(_numPlayersInput, out int numPlayers);
            for (int i = 0; i < numTournaments; i++)
            {
                if (numPlayers == 0)
                {
                    numPlayers = rand.Next(8, 100);
                }
                Tournament tournament = new Tournament(numPlayers, numRounds, numSimultaneousMatches, output_dir, true);
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

                    case "-P":
                        _numPlayersInput = args[++i];
                        break;

                    case "-R":
                        _numRoundsInput = args[++i];
                        // Leaving numRounds to be string so we can re-enter input if corrupt rather than throw exception at beginning
                        break;

                    case "-S":
                        _numSimultaneousMatches = args[++i];
                        // Leaving numSimultaneousMatches to be string so we can re-enter input if corrupt rather than throw exception at beginning
                        break;

                    case "-T":
                        _numTournaments = args[++i];
                        break;
                }
            }
        }
    }
}
