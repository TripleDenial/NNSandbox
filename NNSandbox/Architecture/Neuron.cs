using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.Architecture {
    public abstract class Neuron {
        public string Name { get; }

        public double Input { get; set; } = 0;

        public double Output => ActivationFunction.CalcFunction(Input);

        public IActivationFunction ActivationFunction { get; protected set; }

        public Neuron(string name) {
            Name = name;
        }
    }
}
