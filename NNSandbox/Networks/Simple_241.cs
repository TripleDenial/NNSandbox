using NeuralNetworkSandbox.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkSandbox.Networks {
    public class Simple_241 {
        /*
         *      I1   I2
         *      | \/ |
         *      | /\ |
         *      H1   H2   H3   H4
         *       \   /
         *        \ /
         *         O
         */
        public static NeuralNetwork Create(IActivationFunction func = null) {
            NeuralNetwork network = new();
            InputNeuron i1 = new("I1");
            InputNeuron i2 = new("I2");
            HiddenNeuron h1 = new("H1", func ?? new Sigmoid());
            HiddenNeuron h2 = new("H2", func ?? new Sigmoid());
            HiddenNeuron h3 = new("H3", func ?? new Sigmoid());
            HiddenNeuron h4 = new("H4", func ?? new Sigmoid());
            OutputNeuron o = new("O");

            network.Layers.Add(new(LayerType.Input, i1, i2));
            network.Layers.Add(new(LayerType.Hidden, h1, h2, h3, h4));
            network.Layers.Add(new(LayerType.Output, o));

            Random random = new();
            network.Synaps[i1, h1] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i1, h2] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i1, h3] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i1, h4] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i2, h1] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i2, h2] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i2, h3] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[i2, h4] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[h1, o] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[h2, o] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[h3, o] = (random.NextDouble() - 0.5d) * 2;
            network.Synaps[h4, o] = (random.NextDouble() - 0.5d) * 2;

            return network;
        }
    }
}
