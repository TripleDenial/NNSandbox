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

        //public WeightMatrix Synaps { get; private set; }

        public int IterationCount { get; private set; }

        public int EpochCount { get; private set; }

        public double Momentum { get; set; } = 0;

        public double LearningSpeed { get; set; } = 0.3;

        public double Precision { get; set; } = 0.1;

        public double Result => outputNeuron.Output;

        public Action<string> Log { get; set; }

        public NeuralNetwork() {
            Layers = new List<Layer>();
            //Synaps = new WeightMatrix();
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

        public void RunTrainSet(TrainSet trainSet) {
            Validate();
            SetInputs(trainSet);

            foreach (Layer layer in Layers)
                foreach (Neuron source in layer.Neurons)
                    foreach (Synaps synaps in source.Outgoing)
                        synaps.Target.Input += source.Output * synaps.Weight;
                    
            IterationCount++;
            //Log?.Invoke(OutputAsText);
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
                Log?.Invoke($"Epoch count: {EpochCount}{Environment.NewLine}Set count: {IterationCount}{Environment.NewLine}Loss: {(loss / setCount):0.###}{Environment.NewLine}Accuracy: {(setCountWithCorrectResult / setCount):0.###}");
            }
            EpochCount++;
            return (loss / setCount, setCountWithCorrectResult / setCount);
        }

        private void SetInputs(TrainSet trainSet) {
            foreach (Layer layer in Layers)
                foreach (Neuron neuron in layer.Neurons)
                    neuron.Input = neuron is InputNeuron ? trainSet.InputParameters[neuron.Name] : 0;
        }

        public void Learn(TrainSet trainSet) {
            Dictionary<Neuron, double> deltas = new();
            foreach (Layer layer in Layers.Reverse<Layer>())
                foreach (Neuron neuron in layer.Neurons) {
                    switch(neuron) {
                        case OutputNeuron outputNeuron:
                            deltas.Add(outputNeuron, Result - trainSet.ExpectedResult);
                            break;
                        case InputNeuron inputNeuron:
                            foreach (Synaps synaps in inputNeuron.Outgoing) {
                                AdjustSynaps(synaps, deltas[synaps.Target]);
                            }
                            break;
                        case BiasNeuron biasNeuron:
                            foreach (Synaps synaps in biasNeuron.Outgoing) {
                                AdjustSynaps(synaps, deltas[synaps.Target]);
                            }
                            break;
                        default:
                            double delta = 0;
                            foreach (Synaps synaps in neuron.Outgoing) {
                                delta += synaps.Weight * deltas[synaps.Target];
                                AdjustSynaps(synaps, deltas[synaps.Target]);
                            }
                            delta *= neuron.ActivationFunction.CalcDerivative(neuron.Output);
                            deltas.Add(neuron, delta);
                            break;
                    }
                }
            //Log?.Invoke($"Synapses:{Environment.NewLine}{Synaps}{Environment.NewLine}");
        }

        private void AdjustSynaps(Synaps synaps, double targetDelta) {
            double gradient = synaps.Source.Output * targetDelta;
            double change = gradient * LearningSpeed + Momentum * synaps.LastChange;
            synaps.Weight -= change;
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
                //sb.Append(Synaps);
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
