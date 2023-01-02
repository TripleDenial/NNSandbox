namespace NNSandbox.Architecture {
    public class Synaps {
        private double weight; 

        public Neuron Source { get; }
        public Neuron Target { get; }
        public double Weight {
            get => weight;
            set {
                LastChange = value - weight;
                weight = value;
            }
        }
        public double LastChange { get; private set; } = 0;

        public Synaps(Neuron source, Neuron target, double weight = 0) {
            Source = source;
            Target = target;
            Weight = weight;
        }
    }
}
