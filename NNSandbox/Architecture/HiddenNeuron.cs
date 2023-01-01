using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.Architecture {
    public class HiddenNeuron : Neuron {
        public HiddenNeuron(string name, IActivationFunction normalizeFunction = null) : base(name) {
            ActivationFunction = normalizeFunction ?? new Sigmoid();
        }
    }
}
