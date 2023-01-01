using System.Collections.Generic;

namespace NNSandbox.TrainSets {
    public static class XOR {
        public static Epoch Epoch {
            get {
                TrainSet ts1 = new(new Dictionary<string, double> { { "I1", 0 }, { "I2", 0 } }, 0);
                TrainSet ts2 = new(new Dictionary<string, double> { { "I1", 1 }, { "I2", 0 } }, 1);
                TrainSet ts3 = new(new Dictionary<string, double> { { "I1", 0 }, { "I2", 1 } }, 1);
                TrainSet ts4 = new(new Dictionary<string, double> { { "I1", 1 }, { "I2", 1 } }, 0);

                return new Epoch(new List<TrainSet> { ts1, ts2, ts3, ts4});
            }
        }
    }
}
