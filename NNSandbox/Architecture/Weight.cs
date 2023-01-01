using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.Architecture {
    public class Weight {
        private double val;
        public Neuron Source { get; }
        public Neuron Target { get; }
        public double Value {
            get => val;
            set {
                LastChange = value - val;
                val = value;
            }
        }
        public double LastChange { get; private set; }

        public Weight(Neuron source, Neuron target, double weight) {
            Source = source;
            Target = target;
            LastChange = 0;
            val = weight;
        }
    }
}
