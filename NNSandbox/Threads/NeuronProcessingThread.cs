using NNSandbox.Architecture;
using System.Collections.Concurrent;
using System.Threading;

namespace NNSandbox.Threads {

    public enum Mode { Forward, Backward }

    public class NeuronProcessingThread {
        private Thread thread;

        private bool stop;

        private readonly string threadName;

        public ConcurrentQueue<Neuron> Neurons { get; }

        public EventWaitHandle Signal { get; }

        public EventWaitHandle WorkDone { get; }

        public static double ExpectedResult { get; set; }

        public static double LearnRate { get; set; }

        public static double Momentum { get; set; }

        public static Mode Mode { get; set; }

        public NeuronProcessingThread(string name) {
            threadName = name;
            Neurons = new ConcurrentQueue<Neuron>();
            Signal = new EventWaitHandle(false, EventResetMode.AutoReset);
            WorkDone = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void Start() {
            thread = new Thread(ProcessNeurons) { Name = threadName, IsBackground = true };
            thread.Start();
        }

        public void Stop() {
            stop = true;
            Neurons.Clear();
            Signal.Set();
        }

        private void ProcessNeurons() {
            while (!stop) {
                if (Neurons.TryDequeue(out Neuron neuron)) {
                    switch(Mode) {
                        case Mode.Forward: Forward(neuron); break;
                        case Mode.Backward: Backward(neuron); break;
                    }
                } else {
                    WorkDone.Set();
                    Signal.WaitOne();
                    WorkDone.Reset();
                }
            }
        }

        private static void Forward(Neuron neuron) {
            foreach (Synaps synaps in neuron.Outgoing)
                synaps.Target.Input += neuron.Output * synaps.Weight;
        }

        private static void Backward(Neuron neuron) {
            switch (neuron) {
                case OutputNeuron outputNeuron:
                    outputNeuron.Delta = neuron.Output - ExpectedResult;
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

        private static void AdjustSynaps(Synaps synaps, double targetDelta) {
            synaps.Weight -= synaps.Source.Output * targetDelta * LearnRate + Momentum * synaps.LastChange;
        }
    }
}
