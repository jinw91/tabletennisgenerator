using System;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int numRounds = 30;
            Tournament tournament = new Tournament(13, numRounds, 2, "C:\\output\\tournament");
            tournament.BuildTournament();
            
        }
    }
}
