using Microsoft.Data.Sqlite;
using NNSandbox.Architecture;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NNSandbox.Networks {
    public class Lol1 {
        public static NeuralNetwork Create(IActivationFunction func = null) {
            NeuralNetwork network = new();

            //input layer
            (List<string> team1, List<string> team2) = GetInputLayerNames();
            Layer inputLayer = new();
            foreach(string input in team1) {
                inputLayer.Add(new InputNeuron(input));
            }
            foreach (string input in team2) {
                inputLayer.Add(new InputNeuron(input));
            }
            inputLayer.Add(new BiasNeuron("BI"));
            network.Layers.Add(inputLayer);

            //embedding layer
            Layer embeddingLayer = new();
            for (int i = 0; i < 100; i++) {
                embeddingLayer.Add(new HiddenNeuron($"E1_{i + 1}", func ?? new Sigmoid()));
            }
            for (int i = 0; i < 100; i++) {
                embeddingLayer.Add(new HiddenNeuron($"E2_{i + 1}", func ?? new Sigmoid()));
            }
            embeddingLayer.Add(new BiasNeuron("BE"));
            network.Layers.Add(embeddingLayer);

            //hidden layer
            Layer hiddenLayer = new();
            for (int i = 0; i < 64; i++) {
                hiddenLayer.Add(new HiddenNeuron($"H{i + 1}", func ?? new Sigmoid()));
            }
            hiddenLayer.Add(new BiasNeuron("BH"));
            network.Layers.Add(hiddenLayer);

            //output layer
            OutputNeuron outputNeuron = new("O");
            network.Layers.Add(new(LayerType.Output, outputNeuron));

            //set synaps
            Random random = new();
            foreach(Neuron inputNeuron in inputLayer.Neurons) {
                foreach(HiddenNeuron embeddingNeuron in embeddingLayer.Neurons.OfType<HiddenNeuron>()) {
                    if (inputNeuron.Name[0] == embeddingNeuron.Name[1]) {
                        inputNeuron.AddSynapsTo(embeddingNeuron, (random.NextDouble() - 0.5d) * 2);
                    }
                }
            }

            foreach (Neuron embeddingNeuron in embeddingLayer.Neurons) {
                foreach (HiddenNeuron hiddenNeuron in hiddenLayer.Neurons.OfType<HiddenNeuron>()) {
                    embeddingNeuron.AddSynapsTo(hiddenNeuron, (random.NextDouble() - 0.5d) * 2);
                }
            }

            foreach (Neuron hiddenNeuron in hiddenLayer.Neurons) {
                hiddenNeuron.AddSynapsTo(outputNeuron, (random.NextDouble() - 0.5d) * 2);
            }

            return network;
        }

        private static (List<string>, List<string>) GetInputLayerNames() {
            SqliteConnection sqliteConnection = new(@"DataSource=D:\Projects\LeagueNotepad\dist\Debug\ln.db");
            sqliteConnection.Open();

            List<string> team1 = new(), team2 = new();
            using SqliteCommand command = new($"SELECT name FROM champion", sqliteConnection);
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                team1.Add($"1_{reader.GetString(0)}");
                team2.Add($"2_{reader.GetString(0)}");
            }
            sqliteConnection.Close();
            return (team1, team2);
        }

    }
}
