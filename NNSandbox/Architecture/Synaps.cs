using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.Architecture {
    public class Synaps {
        public Neuron Source { get; }
        public Neuron Target { get; }
        public double Weight { get; set; }

        public Synaps(Neuron source, Neuron target, double weight = 0) {
            Source = source;
            Target = target;
            Weight = weight;
        }
    }
}
