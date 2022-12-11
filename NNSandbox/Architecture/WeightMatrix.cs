using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkSandbox.Architecture {
    public class WeightMatrix {
        private List<Weight> weights;

        public WeightMatrix(List<Weight> weightList = null) {
            weights = weightList == null ? new List<Weight>() : weightList;
        }

        public double this[Neuron sourceNeuron, Neuron targetNeuron] {
            get => HasWeight(sourceNeuron, targetNeuron, out Weight weight) ? weight.Value : 0;
            set {
                if (HasWeight(sourceNeuron, targetNeuron, out Weight weight))
                    weight.Value = value;
                else
                    weights.Add(new Weight(sourceNeuron, targetNeuron, value));
            }
        }

        public bool HasWeight(Neuron sourceNeuron, Neuron targetNeuron, out Weight weight) {
            weight = weights.FirstOrDefault(w => w.Source == sourceNeuron && w.Target == targetNeuron);
            return weight != null;
        }

        public override string ToString() {
            StringBuilder sb = new();
            foreach(Weight weight in weights)
                sb.AppendLine($"{weight.Source.Name}->{weight.Target.Name} = {weight.Value:F3}");
            return sb.ToString();
        }

        public IEnumerable<Weight> WeightsBySource(Neuron sourceNeuron) {
            return weights.Where(w => w.Source == sourceNeuron);
        }

        public IEnumerable<Weight> WeightsByTarget(Neuron targetNeuron) {
            return weights.Where(w => w.Target == targetNeuron);
        }

        public Dictionary<Weight, double> GetGradients(Dictionary<Neuron, double> deltas) {
            return weights.ToDictionary(w => w, w => deltas[w.Target] * w.Source.Output);
        }

        public void ApplyChanges(Dictionary<Weight, double> changes) {
            foreach(Weight weight in weights) {
                weight.Value -= changes[weight];
            }
        }
    }
}
