using System.Collections.Generic;

namespace NNSandbox.TrainSets {
    public class TrainSet {
        private readonly Dictionary<string, double> inputParameters;
        public Dictionary<string, double> InputParameters => inputParameters;

        public double ExpectedResult { get; }

        public TrainSet(Dictionary<string, double> input, double result) {
            ExpectedResult = result;
            inputParameters = input;
        }
    }
}
