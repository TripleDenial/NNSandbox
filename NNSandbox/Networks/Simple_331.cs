using System;
using NNSandbox.Architecture;

namespace NNSandbox.Networks {
    public class Simple_331 {
        /*
         *      I1   I2   BI
         *      | \/ |  /
         *      | /\ | /
         *      H1   H2   BH
         *       \   /   /
         *        \ /   /
         *         O  _/
         */
        public static NeuralNetwork Create(IActivationFunction func = null) {
            NeuralNetwork network = new();
            InputNeuron i1 = new("I1");
            InputNeuron i2 = new("I2");
            HiddenNeuron h1 = new("H1", func ?? new Sigmoid());
            HiddenNeuron h2 = new("H2", func ?? new Sigmoid());
            BiasNeuron bi = new("BI");
            BiasNeuron bh = new("BH");
            OutputNeuron o = new("O");

            network.Layers.Add(new(LayerType.Input, i1, i2, bi));
            network.Layers.Add(new(LayerType.Hidden, h1, h2, bh));
            network.Layers.Add(new(LayerType.Output, o));

            Random random = new();
            i1.AddSynapsTo(h1, (random.NextDouble() - 0.5d) * 2);
            i1.AddSynapsTo(h2, (random.NextDouble() - 0.5d) * 2);
            i2.AddSynapsTo(h1, (random.NextDouble() - 0.5d) * 2);
            i2.AddSynapsTo(h2, (random.NextDouble() - 0.5d) * 2);
            bi.AddSynapsTo(h1, (random.NextDouble() - 0.5d) * 2);
            bi.AddSynapsTo(h2, (random.NextDouble() - 0.5d) * 2);
            h1.AddSynapsTo(o, (random.NextDouble() - 0.5d) * 2);
            h2.AddSynapsTo(o, (random.NextDouble() - 0.5d) * 2);
            bh.AddSynapsTo(o, (random.NextDouble() - 0.5d) * 2);

            return network;
        }
    }
}
