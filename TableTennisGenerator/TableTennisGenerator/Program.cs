using System;

namespace TableTennisGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Tournament tournament = new Tournament(13, 1, 2);
            tournament.BuildRound();

            Console.ReadKey();
        }
    }
}
