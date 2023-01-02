using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNSandbox.Architecture;

namespace NNSandbox.Networks {
    public static class Simple_221 {
        /*
         *      I1   I2
         *      | \/ |
         *      | /\ |
         *      H1   H2
         *       \   /
         *        \ /
         *         O
         */
        public static NeuralNetwork Create(IActivationFunction func = null) {
            NeuralNetwork network = new();
            InputNeuron i1 = new("I1"), i2 = new("I2");
            HiddenNeuron h1 = new("H1", func ?? new Sigmoid()), h2 = new("H2", func ?? new Sigmoid());
            OutputNeuron o = new("O");

            network.Layers.Add(new(LayerType.Input, i1, i2));
            network.Layers.Add(new(LayerType.Hidden, h1, h2));
            network.Layers.Add(new(LayerType.Output, o));

            Random random = new();
            i1.AddSynapsTo(h1, (random.NextDouble() - 0.5d) * 2);
            i1.AddSynapsTo(h2, (random.NextDouble() - 0.5d) * 2);
            i2.AddSynapsTo(h1, (random.NextDouble() - 0.5d) * 2);
            i2.AddSynapsTo(h2, (random.NextDouble() - 0.5d) * 2);
            h1.AddSynapsTo(o, (random.NextDouble() - 0.5d) * 2);
            h2.AddSynapsTo(o, (random.NextDouble() - 0.5d) * 2);

            return network;
        }
    }
}
