using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NNSandbox.Architecture;
using NNSandbox.Threads;
using NNSandbox.TrainSets;

namespace NNSandbox {
    public class NeuralNetwork : IDisposable {
        private const int WORK_THREAD_COUNT = 6;

        private OutputNeuron outputNeuron;

        public List<Layer> Layers { get; }

        public int IterationCount { get; private set; }

        public int EpochCount { get; private set; }

        public double Momentum { get; set; } = 0;

        public double LearningSpeed { get; set; } = 0.3;

        public double Precision { get; set; } = 0.1;

        public double Result => outputNeuron.Output;

        public Action<int, int, double, double> Report { get; set; }

        private NeuronProcessingThread[] workThreads;

        public NeuralNetwork() {
            Layers = new List<Layer>();
            InitThreads();
        }

        private void InitThreads() {
            workThreads = new NeuronProcessingThread[WORK_THREAD_COUNT];
            for (int i = 0; i < WORK_THREAD_COUNT; i++) {
                workThreads[i] = new NeuronProcessingThread($"NeuronWorker{i}");
                workThreads[i].Start();
            }
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
            NeuronProcessingThread.Mode = Mode.Forward;
            int threadId = 0;

            foreach (Layer layer in Layers) {
                foreach (Neuron neuron in layer.Neurons) {
                    workThreads[threadId].Neurons.Enqueue(neuron);
                    workThreads[threadId].Signal.Set();
                    threadId = (++threadId) % WORK_THREAD_COUNT;
                }
                foreach(NeuronProcessingThread workThread in workThreads)
                    workThread.WorkDone.WaitOne();
            }
                
                    
            IterationCount++;
        }

        public (double Loss, double Accuracy) RunEpoch(Epoch epoch, CancellationToken token, bool doLearn = true) {
            NeuronProcessingThread.LearnRate = LearningSpeed;
            NeuronProcessingThread.Momentum = Momentum;
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
            NeuronProcessingThread.Mode = Mode.Backward;
            NeuronProcessingThread.ExpectedResult = trainSet.ExpectedResult;
            int threadId = 0;
            foreach (Layer layer in Layers.Reverse<Layer>()) {
                foreach (Neuron neuron in layer.Neurons) {
                    workThreads[threadId].Neurons.Enqueue(neuron);
                    workThreads[threadId].Signal.Set();
                    threadId = (++threadId) % WORK_THREAD_COUNT;
                }
                foreach (NeuronProcessingThread workThread in workThreads)
                    workThread.WorkDone.WaitOne();
            }
                
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                foreach (NeuronProcessingThread workThread in workThreads)
                    workThread.Stop();
            }
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
