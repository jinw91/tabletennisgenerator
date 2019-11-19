using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TableTennisGenerator
{
 
    class Tournament
    {
        int _numPlayers;
        int _numRounds;
        int _simultaneousMatches;
        Dictionary<int, List<int>> _players;
        Dictionary<string, int> _playCounts;
        Dictionary<string, List<string>> _partners;
        List<string> _playerNames;  // mapping of index (used to reference player in _players) to string name (used in metrics reporting)
        StreamWriter _stream;

        public Tournament(int numPlayers, int numRounds, int simultaneousMatches, string fileDirectory)
        {
            // TODO: error checking of params
            _numPlayers = numPlayers;
            _numRounds = numRounds;
            _simultaneousMatches = simultaneousMatches;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            _stream = new StreamWriter(Path.Combine(fileDirectory, fileName), false);

            _playerNames = new List<string>();
            for (int i=0; i<numPlayers; i++)
            {
                _playerNames.Add(i.ToString());
            }

            _players = InitializeGraph();
            InitializeMetrics();
        }

        public Tournament(List<string> playerNames, int numRounds, int simultaneousMatches, string fileDirectory) : this(playerNames.Count, numRounds, simultaneousMatches, fileDirectory)
        {
            _playerNames = playerNames;
        }

        public void InitializeMetrics()
        {
            _playCounts = new Dictionary<string, int>();
            _partners = new Dictionary<string, List<string>>();

            for (int i = 0; i < _numPlayers; i++)
            {
                _playCounts.Add(_playerNames[i], 0);
                _partners.Add(_playerNames[i], new List<string>());
            }
        }

        public Dictionary<int, List<int>> InitializeGraph()
        {
            Dictionary<int, List<int>> players = new Dictionary<int, List<int>>();
            // construct fully-connected graph w/ numPlayers nodes
            for (int i = 0; i < _numPlayers; i++)  
            {
                List<int> otherPlayers = new List<int>();
                for (int j = 0; j < _numPlayers; j++)
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
            RecordTournamentMetrics();

            for (int i = 0; i < _numRounds; i++)
            {
                BuildRound();
            }

            RecordRoundMetrics();
        }

        public void RecordTournamentMetrics()
        {
            _stream.WriteLine("Number of Players, Number of Rounds, Number of Simultaneous Matches");
            _stream.WriteLine($"{_numPlayers},{_numRounds},{_simultaneousMatches}");
            _stream.WriteLine();
            _stream.WriteLine("Team1-Player1, Team1-Player2, Team2-Player1, Team2-Player2");
        }

        public void RecordRoundMetrics()
        {
            _stream.WriteLine();
            _stream.WriteLine("Player, Partners-->");
            for (int i = 0; i < _numPlayers; i++)
            {
                _stream.Write($"{i},");
                for (int j = 0; j < _partners[_playerNames[i]].Count; j++)
                {
                    _stream.Write($"{_partners[_playerNames[i]][j]},");
                }
                _stream.WriteLine();
            }
            _stream.Close();
        }

        public void BuildRound()
        {
            bool[] visited = new bool[_numPlayers];

            List<List<Tuple<int, int>>> roundMatches = new List<List<Tuple<int, int>>>();

            for (int i=0; i<_simultaneousMatches; i++)
            {
                try
                {
                    List<Tuple<int, int>> teams = BuildMatch(visited);
                    CollectMetrics(teams);
                    roundMatches.Add(teams);
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Resetting graph.");
                    _players = InitializeGraph();
                    foreach (List<Tuple<int, int>> matches in roundMatches)
                    {
                        foreach (Tuple<int, int> pair in matches)
                        {
                            _players[pair.Item1].Remove(pair.Item2);
                            _players[pair.Item2].Remove(pair.Item1);
                        }
                    }
                    i--;  // this match didn't actually return players; it just rebuilt the graph so redo it
                }
            }

        }

        public void CollectMetrics(List<Tuple<int, int>> teams)
        {
            _playCounts[_playerNames[teams[0].Item1]]++;
            _playCounts[_playerNames[teams[0].Item2]]++;
            _playCounts[_playerNames[teams[1].Item1]]++;
            _playCounts[_playerNames[teams[1].Item2]]++;
            _partners[_playerNames[teams[0].Item1]].Add(_playerNames[teams[0].Item2]);
            _partners[_playerNames[teams[0].Item2]].Add(_playerNames[teams[0].Item1]);
            _partners[_playerNames[teams[1].Item1]].Add(_playerNames[teams[1].Item2]);
            _partners[_playerNames[teams[1].Item2]].Add(_playerNames[teams[1].Item1]);

            _stream.WriteLine($"{_playerNames[teams[0].Item1]}, {_playerNames[teams[0].Item2]}, " +
                $"{_playerNames[teams[1].Item1]}, {_playerNames[teams[1].Item2]}");
        }

        public List<Tuple<int, int>> BuildMatch(bool[] visited)
        {
            bool[] visitedOG = new bool[visited.Length];
            Array.Copy(visited, visitedOG, visited.Length);
            Tuple<int, int> team1 = FindTeam(visited);
            Tuple<int, int> team2;
            try
            {
                team2 = FindTeam(visited);
            }
            catch (InvalidOperationException)
            {
                visited[team1.Item1] = false;
                visited[team1.Item2] = false;
                throw;
            }
            _players[team1.Item1].Remove(team1.Item2);
            _players[team1.Item2].Remove(team1.Item1);
            _players[team2.Item1].Remove(team2.Item2);
            _players[team2.Item2].Remove(team2.Item1);

            return new List<Tuple<int, int>> { team1, team2 };
        }

        public Tuple<int, int> FindTeam(bool[] visited)
        {
            int starter = FindMostConnectedPlayer(visited);
            if (starter == -1)
            {
                throw new InvalidOperationException();
            }
            int partner = FindPartner(starter, visited);
            if (partner == -1)
            {
                throw new InvalidOperationException();
            }
            visited[starter] = true;
            visited[partner] = true;
            return new Tuple<int, int>(starter, partner);
        }

        public int FindMostConnectedPlayer(bool[] visited)
        {
            return FindRandomMaxPlayer(_players.Keys.ToList(), visited);
        }

        public int FindPartner(int player, bool[] visited)
        { 
            return FindRandomMaxPlayer(_players[player], visited);
        }

        public int FindRandomMaxPlayer(List<int> possiblePlayers, bool[] visited)
        {
            int max = 0;
            List<int> maxPlayers = new List<int>();
            foreach (int p in possiblePlayers)
            {
                if (_players[p].Count > 0 && !visited[p])
                {
                    if (_players[p].Count == max)
                    {
                        maxPlayers.Add(p);
                    }
                    else if (_players[p].Count > max)
                    {
                        maxPlayers = new List<int> { p };
                        max = _players[p].Count;
                    }
                }
            }
            int mostConnectedPlayer = -1;
            if (maxPlayers.Count > 0)
            {
                Random rand = new Random();
                mostConnectedPlayer = maxPlayers[rand.Next(maxPlayers.Count)];
            }
            return mostConnectedPlayer;
        }
        
    }
}
