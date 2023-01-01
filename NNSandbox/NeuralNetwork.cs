using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNSandbox.Architecture;
using NNSandbox.TrainSets;

namespace NNSandbox {
    public class NeuralNetwork {
        private OutputNeuron outputNeuron;

        public List<Layer> Layers { get; }

        public WeightMatrix Synaps { get; private set; }

        public int IterationCount { get; private set; }

        public int EpochCount { get; private set; }

        public double Momentum { get; set; } = 0;

        public double LearningSpeed { get; set; } = 0.3;

        public double Precision { get; set; } = 0.1;

        public double Result => outputNeuron.Output;

        public Action<string> Log { get; set; }

        public NeuralNetwork() {
            Layers = new List<Layer>();
            Synaps = new WeightMatrix();
        }

        public void Validate() {
            List<string> neuronNames = new();
            Layer inputLayer = null, outputLayer = null;

            if(!Layers.Any())
                throw new Exception("Network has no Layers!");

            foreach (Layer layer in Layers) {
                if (!layer.Neurons.Any())
                    throw new Exception("Network contains an empty layer!");
                if (layer.Neurons.Select(n => n.Name).Intersect(neuronNames).Any())
                    throw new Exception("Network has neurons with the same names!");
                else
                    neuronNames.AddRange(layer.Neurons.Select(n => n.Name));
                if (layer.Type == LayerType.Input)
                    inputLayer = layer;
                if (layer.Type == LayerType.Output)
                    outputLayer = layer;
            }

            if(inputLayer == null)
                throw new Exception("Network does not have input layer!");
            if(Layers.First() != inputLayer) {
                Layers.Remove(inputLayer);
                Layers.Insert(0, inputLayer);
            }            

            if (outputLayer == null)
                throw new Exception("Network does not have output layer!");
            if(Layers.Last() != outputLayer) {
                Layers.Remove(outputLayer);
                Layers.Add(outputLayer);
            }
            outputNeuron = outputLayer.Neurons.First() as OutputNeuron;
        }

        public void InitializeSynaps(List<(Neuron, Neuron)> pairs) {
            Synaps = new WeightMatrix();
            Random random = new();
            foreach (var pair in pairs)
                Synaps[pair.Item1, pair.Item2] = random.NextDouble();
        }

        public void RunTrainSet(TrainSet trainSet) {
            Validate();
            SetInputs(trainSet);

            foreach(Layer layer in Layers)
                foreach(Neuron source in layer.Neurons)
                    foreach(Weight weight in Synaps.WeightsBySource(source))
                        weight.Target.Input += source.Output * weight.Value;
                    
            IterationCount++;
            Log?.Invoke(OutputAsText);
        }

        public (double, double) RunEpoch(Epoch epoch) {
            double loss = 0;
            int setCount = 0;
            double setCountWithCorrectResult = 0;
            foreach (TrainSet trainSet in epoch.TrainSets) {
                RunTrainSet(trainSet);
                if (Math.Abs(trainSet.ExpectedResult - Result) < Precision)
                    setCountWithCorrectResult++;
                loss += Math.Pow(trainSet.ExpectedResult - Result, 2);
                setCount++;
                Learn(trainSet);
            }
            EpochCount++;
            return (loss / setCount, setCountWithCorrectResult / setCount);
        }

        private void SetInputs(TrainSet trainSet) {
            foreach (Layer layer in Layers)
                foreach (Neuron neuron in layer.Neurons)
                    neuron.Input = 0;

            Layer inputLayer = Layers.First();
            foreach (var inputParam in trainSet.InputParameters) {
                Neuron neuron = inputLayer.Neurons.FirstOrDefault(n => n.Name == inputParam.Key);
                if (neuron is InputNeuron inputNeuron)
                    inputNeuron.Input = inputParam.Value;
            }
        }

        public void Learn(TrainSet trainSet) {
            Dictionary<Neuron, double> deltas = EvaluateDeltas(trainSet.ExpectedResult);
            Dictionary<Weight, double> gradients = Synaps.GetGradients(deltas);
            Dictionary<Weight, double> changes = gradients.ToDictionary(kvp => kvp.Key, kvp => kvp.Value * LearningSpeed + Momentum * kvp.Key.LastChange);
            Synaps.ApplyChanges(changes);
            Log?.Invoke($"Synapses:{Environment.NewLine}{Synaps}{Environment.NewLine}");
        }

        private Dictionary<Neuron, double> EvaluateDeltas(double expectedResult) {
            Dictionary<Neuron, double> deltas = new();
            foreach (Layer layer in Layers.Reverse<Layer>())
                foreach (Neuron neuron in layer.Neurons) {
                    double delta = 0;
                    if (layer.Type == LayerType.Output)
                        delta = Result - expectedResult;
                    else 
                        foreach (Weight weight in Synaps.WeightsBySource(neuron))
                            delta += weight.Value * deltas[weight.Target];
                    delta *= neuron.ActivationFunction.CalcDerivative(neuron.Output);
                    deltas.Add(neuron, delta);
                }
            return deltas;
        }

        public string ArchitectureAsText {
            get {
                StringBuilder sb = new();
                sb.AppendLine($"Momentum: {Momentum}");
                sb.AppendLine($"LearningSpeed: {LearningSpeed}");
                sb.AppendLine("Neurons:");
                foreach (Layer layer in Layers)
                    sb.Append(layer.ToString());                
                sb.AppendLine($"Synapses:");
                sb.Append(Synaps);
                return sb.ToString();
            }
        }

        public string OutputAsText {
            get {
                StringBuilder sb = new();
                sb.AppendLine($"Outputs({IterationCount}):");
                foreach (Layer layer in Layers) {
                    foreach (Neuron neuron in layer.Neurons)
                        sb.Append($"{neuron.Output:F3} ");
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }
    }
}
