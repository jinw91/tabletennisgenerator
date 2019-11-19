using System;
using System.Collections.Generic;
using System.Text;

namespace TableTennisGenerator
{
 
    class Tournament
    {
        int numPlayers;
        int numRounds;
        int simultaneousMatches;
        Dictionary<int, List<int>> players;

        public Tournament(int numPlayers, int numRounds, int simultaneousMatches)
        {
            // TODO: error checking of params
            this.numPlayers = numPlayers;
            this.numRounds = numRounds;
            this.simultaneousMatches = simultaneousMatches;

            players = InitializeGraph();
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

        public void BuildRound()
        {
            bool[] visited = new bool[numPlayers];

            List<List<Tuple<int, int>>> roundMatches = new List<List<Tuple<int, int>>>();

            for (int i=0; i<simultaneousMatches; i++)
            {
                try
                {
                    roundMatches.Add(BuildMatch(visited));
                }
                catch
                {
                    InitializeGraph();
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
            List<int> potentials = new List<int>();
            foreach (int p in players[player])
            {
                if (!visited[p])
                {
                    potentials.Add(p);
                }
            }
            Random rand = new Random();
            return potentials[rand.Next(potentials.Count)];
        }
        
    }
}
