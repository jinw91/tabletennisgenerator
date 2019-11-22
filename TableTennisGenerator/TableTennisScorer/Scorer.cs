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
                        if (!_playerMetrics.ContainsKey(parts[FIRST_NAME]))
                        {
                            InitializePlayer(parts[FIRST_NAME]);
                        }
                        if (!_playerMetrics.ContainsKey(parts[FIRST_PARTNER_NAME]))
                        {
                            InitializePlayer(parts[FIRST_PARTNER_NAME]);
                        }
                        if (!_playerMetrics.ContainsKey(parts[SECOND_NAME]))
                        {
                            InitializePlayer(parts[SECOND_NAME]);
                        }
                        if (!_playerMetrics.ContainsKey(parts[SECOND_PARTNER_NAME]))
                        {
                            InitializePlayer(parts[SECOND_PARTNER_NAME]);
                        }

                        if (firstScore > secondScore)
                        {
                            SetMetrics(parts[FIRST_NAME], parts[FIRST_PARTNER_NAME], parts[SECOND_NAME], parts[SECOND_PARTNER_NAME], firstScore, secondScore);
                        }
                        else
                        {
                            SetMetrics(parts[SECOND_NAME], parts[SECOND_PARTNER_NAME], parts[FIRST_NAME], parts[FIRST_PARTNER_NAME], secondScore, firstScore);
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

        private void InitializePlayer(string player)
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>();
            metrics.Add(GamesPlayed, 0);
            metrics.Add(GamesWon, 0);
            metrics.Add(PointsScored, 0);
            metrics.Add(PointsLost, 0);

            _playerMetrics.Add(player, metrics);
        }

        private void SetMetrics(string player1, string partner1, string player2, string partner2, int winningScore, int losingScore)
        {
            _playerMetrics[player1][GamesPlayed]++;
            _playerMetrics[partner1][GamesPlayed]++;
            _playerMetrics[player2][GamesPlayed]++;
            _playerMetrics[partner2][GamesPlayed]++;

            _playerMetrics[player1][GamesWon]++;
            _playerMetrics[partner1][GamesWon]++;

            _playerMetrics[player1][PointsScored] += winningScore;
            _playerMetrics[partner1][PointsScored] += winningScore;

            _playerMetrics[player2][PointsScored] += losingScore;
            _playerMetrics[partner2][PointsScored] += losingScore;

            _playerMetrics[player1][PointsLost] += losingScore;
            _playerMetrics[partner1][PointsLost] += losingScore;

            _playerMetrics[player2][PointsLost] += winningScore;
            _playerMetrics[partner2][PointsLost] += winningScore;
        }
    }
}
