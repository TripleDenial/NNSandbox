using System.Collections.Generic;

namespace NNSandbox.Architecture {
    public abstract class Neuron {
        public string Name { get; }

        public double Input { get; set; } = 0;

        public double Output => ActivationFunction.CalcFunction(Input);

        public IActivationFunction ActivationFunction { get; protected set; }

        public List<Synaps> Outgoing { get; } = new List<Synaps>();

        public Neuron(string name) {
            Name = name;
        }

        public void AddSynapsTo(Neuron target, double weight = 0) {
            Outgoing.Add(new Synaps(this, target, weight));
        }
    }
}
