using Microsoft.Data.Sqlite;
using NeuralNetworkSandbox.TrainSets;
using System.Collections.Generic;

namespace NNSandbox.TrainSets {
    public static class LolGames {

        public static Epoch GetEpoch(int rank = -1) {
            
            SqliteConnection sqliteConnection = new("DataSource=");
            sqliteConnection.Open();

            Dictionary<string, double> blankInput = CreateBlankInput(sqliteConnection);

            List<TrainSet> trainSets = new();
            using (SqliteCommand command = new($"SELECT winner,team1,team2 FROM gameData WHERE rank > {rank-2} AND rank < {rank+2}", sqliteConnection)) {
                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    trainSets.Add(CreateTrainSetFromData(reader, blankInput));
                }
            }
            sqliteConnection.Close();

            return new Epoch(trainSets);
        }

        private static Dictionary<string, double> CreateBlankInput(SqliteConnection connection) {
            Dictionary<string, double> blankInput = new();
            using SqliteCommand command = new($"SELECT name FROM champion", connection);
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                blankInput[$"1_{reader.GetString(0)}"] = 0;
                blankInput[$"2_{reader.GetString(0)}"] = 0;
            }
            return blankInput;
        }

        private static TrainSet CreateTrainSetFromData(SqliteDataReader reader, Dictionary<string, double> blankInput) {
            Dictionary<string, double> input = new(blankInput);
            foreach (string champ in reader.GetString(1).Split(',')) {
                input[$"1_{champ}"] = 1;
            }
            foreach (string champ in reader.GetString(2).Split(',')) {
                input[$"2_{champ}"] = 1;
            }
            return new TrainSet(input, reader.GetInt32(0) - 1);
        }
    }
}
