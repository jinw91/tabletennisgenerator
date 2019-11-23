using System;
using TableTennisGenerator;

namespace TestTournamentBuilds
{
    class Program
    {
        static void Main(string[] args)
        {
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
            for (int i = 0; i < numTournaments; i++)
            {
                int numPlayers = rand.Next(8, 100);
                Tournament tournament = new Tournament(numPlayers, numRounds, 2, output_dir, true);
                tournament.BuildTournament();
            }
        }
    }
}
