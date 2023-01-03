using System.Collections.Generic;

namespace NNSandbox.TrainSets {
    public class Epoch {
        public List<TrainSet> TrainSets { get; } = new();

        public List<TrainSet> TestSets { get; } = new();

        public Epoch(List<TrainSet> sets) {
            int trainSetsCount = sets.Count * 8 / 10;
            int i = 0;
            foreach(TrainSet set in sets) {
                if (i == sets.Count)
                    break;
                if(i < trainSetsCount) {
                    TrainSets.Add(set);
                } else {
                    TestSets.Add(set);
                }
                i++;
            }
        }
    }
}
