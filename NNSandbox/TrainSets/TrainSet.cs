using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkSandbox.TrainSets {
    public class TrainSet {
        private Dictionary<string, double> inputParameters;

        public double ExpectedResult { get; }

        public IEnumerable<KeyValuePair<string, double>> InputParameters => inputParameters.Where(kvp => true);

        public TrainSet(Dictionary<string, double> input, double result) {
            ExpectedResult = result;
            inputParameters = input;
        }
    }
}
