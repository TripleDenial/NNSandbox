using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkSandbox.Architecture {
    public class InputNeuron : Neuron {
        public InputNeuron(string name) : base(name) {
            ActivationFunction = new Linear();
        }
    }
}
