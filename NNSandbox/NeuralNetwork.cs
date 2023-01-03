using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NNSandbox.Architecture;
using NNSandbox.TrainSets;

namespace NNSandbox {
    public class NeuralNetwork {
        private OutputNeuron outputNeuron;

        public List<Layer> Layers { get; }

        public int IterationCount { get; private set; }

        public int EpochCount { get; private set; }

        public double Momentum { get; set; } = 0;

        public double LearningSpeed { get; set; } = 0.3;

        public double Precision { get; set; } = 0.1;

        public double Result => outputNeuron.Output;

        public Action<int, int, double, double> Report { get; set; }

        public NeuralNetwork() {
            Layers = new List<Layer>();
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
            SetInputs(trainSet);

            foreach (Layer layer in Layers)
                foreach (Neuron source in layer.Neurons)
                    foreach (Synaps synaps in source.Outgoing)
                        synaps.Target.Input += source.Output * synaps.Weight;
                    
            IterationCount++;
        }

        public (double Loss, double Accuracy) RunEpoch(Epoch epoch, CancellationToken token, bool doLearn = true) {
            double loss = 0;
            int setCount = 0;
            double setCountWithCorrectResult = 0;
            List<TrainSet> setsToRun = doLearn ? epoch.TrainSets : epoch.TestSets;
            foreach (TrainSet trainSet in setsToRun) {
                if (token.IsCancellationRequested)
                    break;

                RunTrainSet(trainSet);
                setCount++;
                if (Math.Abs(trainSet.ExpectedResult - Result) < Precision)
                    setCountWithCorrectResult++;
                loss += Math.Pow(trainSet.ExpectedResult - Result, 2);

                if(doLearn)
                    Learn(trainSet);
                
                if(IterationCount % 100 == 0)
                    Report?.Invoke(EpochCount, IterationCount, loss / setCount, setCountWithCorrectResult / setCount);
            }
            EpochCount++;
            return (loss / setCount, setCountWithCorrectResult / setCount);
        }

        public double TestEpoch(Epoch epoch, CancellationToken token) {
            return RunEpoch(epoch, token, false).Accuracy;
        }

        private void SetInputs(TrainSet trainSet) {
            foreach (Layer layer in Layers)
                foreach (Neuron neuron in layer.Neurons) {
                    neuron.Reset();
                    neuron.Input = neuron is InputNeuron ? trainSet.InputParameters[neuron.Name] : 0;
                }   
        }

        public void Learn(TrainSet trainSet) {
            foreach (Layer layer in Layers.Reverse<Layer>())
                foreach (Neuron neuron in layer.Neurons) {
                    switch(neuron) {
                        case OutputNeuron outputNeuron:
                            outputNeuron.Delta = Result - trainSet.ExpectedResult;
                            break;
                        case InputNeuron inputNeuron:
                            foreach (Synaps synaps in inputNeuron.Outgoing) {
                                AdjustSynaps(synaps, synaps.Target.Delta);
                            }
                            break;
                        case BiasNeuron biasNeuron:
                            foreach (Synaps synaps in biasNeuron.Outgoing) {
                                AdjustSynaps(synaps, synaps.Target.Delta);
                            }
                            break;
                        default:
                            foreach (Synaps synaps in neuron.Outgoing) {
                                neuron.Delta += synaps.Weight * synaps.Target.Delta;
                                AdjustSynaps(synaps, synaps.Target.Delta);
                            }
                            neuron.Delta *= neuron.ActivationFunction.CalcDerivative(neuron.Output);
                            break;
                    }
                }
        }

        private void AdjustSynaps(Synaps synaps, double targetDelta) {
            synaps.Weight -= synaps.Source.Output * targetDelta * LearningSpeed + Momentum * synaps.LastChange;
        }

        public string ArchitectureAsText {
            get {
                StringBuilder sb = new();
                sb.AppendLine($"Momentum: {Momentum}");
                sb.AppendLine($"LearningSpeed: {LearningSpeed}");
                sb.AppendLine("Neurons:");
                foreach (Layer layer in Layers)
                    sb.Append(layer.ToString());
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
