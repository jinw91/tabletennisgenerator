using System;
using System.Collections.Generic;
using System.IO;

namespace TableTennisGenerator
{
 
    class Tournament
    {
        int numPlayers;
        int numRounds;
        int simultaneousMatches;
        Dictionary<int, List<int>> players;
        Dictionary<int, int> playCounts;
        Dictionary<int, List<int>> partners;
        StreamWriter stream;

        public Tournament(int numPlayers, int numRounds, int simultaneousMatches, string fileDirectory)
        {
            // TODO: error checking of params
            this.numPlayers = numPlayers;
            this.numRounds = numRounds;
            this.simultaneousMatches = simultaneousMatches;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            stream = new StreamWriter(Path.Combine(fileDirectory, fileName), false);

            players = InitializeGraph();
            InitializeMetrics();
        }

        public void InitializeMetrics()
        {
            playCounts = new Dictionary<int, int>();
            partners = new Dictionary<int, List<int>>();

            for (int i = 0; i < numPlayers; i++)
            {
                playCounts.Add(i, 0);
                partners.Add(i, new List<int>());
            }
        }

        public Dictionary<int, List<int>> InitializeGraph()
        {
            Dictionary<int, List<int>> players = new Dictionary<int, List<int>>();
            // construct fully-connected graph w/ numPlayers nodes
            for (int i = 0; i < numPlayers; i++)  // TODO: switch to actual player names
            {
                List<int> otherPlayers = new List<int>();
                for (int j = 0; j < numPlayers; j++)
                {
                    if (j != i)
                    {
                        otherPlayers.Add(j);
                    }
                }
                players.Add(i, otherPlayers);
            }
            return players;
        }

        public void BuildTournament()
        {
            stream.WriteLine($"{numPlayers},{numRounds},{simultaneousMatches}");

            for (int i = 0; i < numRounds; i++)
            {
                BuildRound();
            }

            for (int i=0; i<numPlayers; i++)
            {
                Console.WriteLine($"Player {i}: {playCounts[i]} games played");
                stream.Write($"{i},");
                Console.WriteLine("Partners: ");
                for (int j = 0; j < partners[i].Count; j++)
                {
                    Console.Write($"{partners[i][j]}, ");
                    stream.Write($"{partners[i][j]},");
                }
                Console.WriteLine();
                stream.WriteLine();
            }
            stream.Close();
        }

        public void BuildRound()
        {
            bool[] visited = new bool[numPlayers];

            List<List<Tuple<int, int>>> roundMatches = new List<List<Tuple<int, int>>>();

            for (int i=0; i<simultaneousMatches; i++)
            {
                try
                {
                    List<Tuple<int, int>> teams = BuildMatch(visited);
                    CollectMetrics(teams);
                    Console.WriteLine($"{teams[0].Item1} and {teams[0].Item2} vs {teams[1].Item1} and {teams[1].Item2}");
                    roundMatches.Add(teams);
                }
                catch
                {
                    Console.WriteLine("Resetting graph.");
                    players = InitializeGraph();
                    foreach (List<Tuple<int, int>> matches in roundMatches)
                    {
                        foreach (Tuple<int, int> pair in matches)
                        {
                            players[pair.Item1].Remove(pair.Item2);
                            players[pair.Item2].Remove(pair.Item1);
                        }
                    }
                    i--;  // this match didn't actually return players; it just rebuilt the graph so redo it
                }
            }

        }

        public void CollectMetrics(List<Tuple<int, int>> teams)
        {
            playCounts[teams[0].Item1]++;
            playCounts[teams[0].Item2]++;
            playCounts[teams[1].Item1]++;
            playCounts[teams[1].Item2]++;
            partners[teams[0].Item1].Add(teams[0].Item2);
            partners[teams[0].Item2].Add(teams[0].Item1);
            partners[teams[1].Item1].Add(teams[1].Item2);
            partners[teams[1].Item2].Add(teams[1].Item1);
        }

        public List<Tuple<int, int>> BuildMatch(bool[] visited)
        {
            Tuple<int, int> team1 = FindTeam(visited);
            Tuple<int, int> team2;
            try
            {
                team2 = FindTeam(visited);
            }
            catch
            {
                visited[team1.Item1] = false;
                visited[team1.Item2] = false;
                throw;
            }
            players[team1.Item1].Remove(team1.Item2);
            players[team1.Item2].Remove(team1.Item1);
            players[team2.Item1].Remove(team2.Item2);
            players[team2.Item2].Remove(team2.Item1);

            return new List<Tuple<int, int>> { team1, team2 };
        }

        public Tuple<int, int> FindTeam(bool[] visited)
        {
            int starter = FindMostConnectedPlayer(visited);
            if (starter == -1)
            {
                throw new Exception();
            }
            int partner = FindPartner(starter, visited);
            visited[starter] = true;
            visited[partner] = true;
            return new Tuple<int, int>(starter, partner);
        }

        public int FindMostConnectedPlayer(bool[] visited)
        {
            int max = 0;
            int maxPlayer = -1;
            foreach (KeyValuePair<int, List<int>> player in players)
            {
                if (player.Value.Count > max && !visited[player.Key])
                {
                    max = player.Value.Count;
                    maxPlayer = player.Key;
                }
            }
            return maxPlayer;
        }

        public int FindPartner(int player, bool[] visited)
        {
            int partner = -1;
            int max = 0;
            foreach (int p in players[player])
            {
                if (!visited[p] && players[p].Count > max)
                {
                    partner = p;
                    max = players[p].Count;
                }
            }
            return partner;
        }
        
    }
}
