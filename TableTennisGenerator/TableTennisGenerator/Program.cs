using System;
using System.Collections.Generic;
using System.IO;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
