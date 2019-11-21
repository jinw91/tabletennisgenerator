using System;
using System.Collections.Generic;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int numRounds = 10;
            List<string> players = new List<string> { "Kate", "Mitchell", "Guanda", "Gaurav", "Nanhua", "Maor", "Tianguang", "Olivia", "Weiye", "Vikas", "Yogesh", "Bill", "Sophie" }; // "i", "j", "k", "l", "m" };

            Tournament tournament = new Tournament("C:\\output\\tournament\\mixed_column_row_names.csv", numRounds, 2, "C:\\output\\tournament");
            tournament.BuildTournament();
            
        }
    }
}
