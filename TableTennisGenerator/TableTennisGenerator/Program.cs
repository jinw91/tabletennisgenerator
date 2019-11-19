using System;
using System.Collections.Generic;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int numRounds = 30;
            List<string> players = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m" };

            Tournament tournament = new Tournament(players, numRounds, 2, "C:\\output\\tournament");
            tournament.BuildTournament();
            
        }
    }
}
