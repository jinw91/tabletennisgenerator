using System;
using System.Collections.Generic;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            for (int i=0; i<10; i++)
            {
                int numPlayers = rand.Next(8, 100);
                int numRounds = 30;
                Tournament tournament = new Tournament(numPlayers, numRounds, 2, "C:\\output\\tournament", true);
                tournament.BuildTournament();
            }
        }
    }
}
