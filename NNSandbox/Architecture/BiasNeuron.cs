using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.Architecture {
    public class BiasNeuron : Neuron {
        public BiasNeuron(string name) : base(name) {
            ActivationFunction = new ConstantOne();
        }
    }
}
