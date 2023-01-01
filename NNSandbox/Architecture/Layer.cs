using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkSandbox.Architecture {
    public enum LayerType { Undefined, Input, Hidden, Output }
    public class Layer {
        private readonly HashSet<Neuron> neurons;
        public LayerType Type { get; private set; }

        public IEnumerable<Neuron> Neurons => neurons.Select(n => n);

        public Layer() {
            Type = LayerType.Undefined;
            neurons = new HashSet<Neuron>();
        }

        public Layer(LayerType type, IEnumerable<Neuron> neuronsToAdd) {
            Type = type;
            neurons = new HashSet<Neuron>(neuronsToAdd);
        } 

        public Layer(LayerType type, params Neuron[] neuronsToAdd) {
            Type = type;
            neurons = new HashSet<Neuron>(neuronsToAdd);
        }

        public void Add(Neuron n) {
            switch(Type) {
                case LayerType.Input:
                    if (!(n is InputNeuron) && !(n is BiasNeuron))
                        throw new Exception($"Cannot add {n.GetType()} to input layer!");
                    neurons.Add(n);
                    break;
                case LayerType.Hidden:
                    if (!(n is HiddenNeuron) && !(n is BiasNeuron))
                        throw new Exception($"Cannot add {n.GetType()} neuron to intermediate layer!");
                    neurons.Add(n);
                    break;
                case LayerType.Output:
                    if (!(n is OutputNeuron))
                        throw new Exception($"Cannot add {n.GetType()} neuron to output layer!");
                    if(neurons.Any())
                        throw new Exception($"Cannot add more than 1 neuron to output layer!");
                    neurons.Add(n);
                    break;
                case LayerType.Undefined:
                    AddWhileUndefined(n);
                    break;
            }
        }

        public override string ToString() {
            StringBuilder sb = new();
            foreach (Neuron neuron in Neurons)
                sb.Append($"{neuron.Name}({neuron.ActivationFunction.GetType().Name})   ");
            sb.AppendLine();
            return sb.ToString();
        }

        private void AddWhileUndefined(Neuron n) {
            switch (n) {
                case InputNeuron input:
                    neurons.Add(input);
                    Type = LayerType.Input;
                    break;
                case BiasNeuron bias:
                    neurons.Add(bias);
                    break;
                case OutputNeuron output:
                    if(neurons.Any())
                        throw new Exception($"Cannot add {n.GetType()} to non-empty layer!");
                    neurons.Add(output);
                    Type = LayerType.Output;
                    break;
                case HiddenNeuron hidden:
                    neurons.Add(hidden);
                    Type = LayerType.Hidden;
                    break;
            }
        }
    }
}
