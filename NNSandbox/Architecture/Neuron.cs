using System.Collections.Generic;

namespace NNSandbox.Architecture {
    public abstract class Neuron {
        public string Name { get; }

        public double Input { get; set; } = 0;

        private bool calculated;
        private double output; 
        public double Output {
            get {
                if (!calculated) {
                    output = ActivationFunction.CalcFunction(Input);
                    calculated = true;
                }
                return output;
            }
        }

        public double Delta { get; set; }

        public IActivationFunction ActivationFunction { get; protected set; }

        public List<Synaps> Outgoing { get; } = new List<Synaps>();

        public Neuron(string name) {
            Name = name;
        }

        public void AddSynapsTo(Neuron target, double weight = 0) {
            Outgoing.Add(new Synaps(this, target, weight));
        }

        public void Reset() {
            calculated = false;
            Delta = 0;
        }
    }
}
