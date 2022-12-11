using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkSandbox.TrainSets {
    public class Epoch {
        private readonly List<TrainSet> trainSets;
        public IEnumerable<TrainSet> TrainSets => trainSets.Where(ts => true);

        public Epoch(List<TrainSet> trainSets) {
            this.trainSets = trainSets;
        }
    }
}
