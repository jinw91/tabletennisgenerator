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
            List<string> players = new List<string> { "Kate", "Mitchell", "Guanda", "Gaurav", "Nanhua", "Maor", "Tianguang", "Olivia", "Weiye", "Vikas", "Yogesh", "Bill" }; // "i", "j", "k", "l", "m" };

            Tournament tournament = new Tournament(players, numRounds, 2, "C:\\output\\tournament");
            tournament.BuildTournament();
            
        }
    }
}
