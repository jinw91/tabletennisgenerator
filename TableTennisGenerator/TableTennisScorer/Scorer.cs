using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TableTennisScorer
{
    public class Scorer
    {
        private const int FIRST_NAME = 2;
        private const int FIRST_PARTNER_NAME = 3;
        private const int SECOND_NAME = 4;
        private const int SECOND_PARTNER_NAME = 5;
        private const int FIRST_SCORE = 6;
        private const int SECOND_SCORE = 7;

        private const string GamesPlayed = "GamesPlayed";
        private const string GamesWon = "GamesWon";
        private const string PointsScored = "PointsScored";
        private const string PointsLost = "PointsLost";

        private const string PointsPerGame = "PointsPerGame";
        private const string PointDifferential = "PointDifferential";

        private string _filePath;
        private Dictionary<string, Dictionary<string, double>> _playerMetrics;
        public Scorer(string filePath)
        {
            _filePath = filePath;
            _playerMetrics = new Dictionary<string, Dictionary<string, double>>();
        }

        public Dictionary<string, Dictionary<string, double>> GenerateMetrics()
        {
            using (StreamReader reader = new StreamReader(_filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(",");

                    if (parts.Length == 8 && int.TryParse(parts[FIRST_SCORE], out int firstScore) && int.TryParse(parts[SECOND_SCORE], out int secondScore))
                    {
                        List<string> names = new List<string>
                        {
                            parts[FIRST_NAME].Trim(),
                            parts[FIRST_PARTNER_NAME].Trim(),
                            parts[SECOND_NAME].Trim(),
                            parts[SECOND_PARTNER_NAME].Trim()
                        };

                        foreach (string name in names)
                        {
                            if (!_playerMetrics.ContainsKey(name))
                            {
                                InitializePlayer(name);
                            }
                        }

                        if (firstScore > secondScore)
                        {
                            SetMetrics(names[0], names[1], names[2], names[3], firstScore, secondScore);
                        }
                        else
                        {
                            SetMetrics(names[2], names[3], names[0], names[1], secondScore, firstScore);
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, double>> keyValuePair in _playerMetrics)
            {
                Dictionary<string, double> metrics = keyValuePair.Value;
                metrics[PointsPerGame] = metrics[PointsScored] / metrics[GamesPlayed];
                metrics[PointDifferential] = metrics[PointsScored] - metrics[PointsLost];
            }

            return _playerMetrics;
        }

        public void WriteOutput(string outputFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(outputFile, false))
            {
                streamWriter.WriteLine("Player,Games Played,Games Won,Points Scored,Points Lost,Points Per Game,Point Differential");
                foreach (KeyValuePair<string, Dictionary<string, double>> keyValuePair in _playerMetrics)
                {
                    streamWriter.Write($"{keyValuePair.Key},");
                    foreach (KeyValuePair<string, double> propertyValuePair in keyValuePair.Value)
                    {
                        streamWriter.Write($"{propertyValuePair.Value},");
                    }
                    streamWriter.Write("\n");
                }
            }
        }

        private void InitializePlayer(string player)
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>();
            metrics.Add(GamesPlayed, 0);
            metrics.Add(GamesWon, 0);
            metrics.Add(PointsScored, 0);
            metrics.Add(PointsLost, 0);

            _playerMetrics.Add(player, metrics);
        }

        private void SetMetrics(string winner1, string winner2, string loser1, string loser2, int winningScore, int losingScore)
        {
            _playerMetrics[winner1][GamesPlayed]++;
            _playerMetrics[winner2][GamesPlayed]++;
            _playerMetrics[loser1][GamesPlayed]++;
            _playerMetrics[loser2][GamesPlayed]++;

            _playerMetrics[winner1][GamesWon]++;
            _playerMetrics[winner2][GamesWon]++;

            _playerMetrics[winner1][PointsScored] += winningScore;
            _playerMetrics[winner2][PointsScored] += winningScore;

            _playerMetrics[loser1][PointsScored] += losingScore;
            _playerMetrics[loser2][PointsScored] += losingScore;

            _playerMetrics[winner1][PointsLost] += losingScore;
            _playerMetrics[winner2][PointsLost] += losingScore;

            _playerMetrics[loser1][PointsLost] += winningScore;
            _playerMetrics[loser2][PointsLost] += winningScore;
        }
    }
}
