using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSandbox.TrainSets {
    public static class OR {
        public static Epoch Epoch {
            get {
                TrainSet ts1 = new(new Dictionary<string, double> { { "I1", 0 }, { "I2", 0 } }, 0);
                TrainSet ts2 = new(new Dictionary<string, double> { { "I1", 1 }, { "I2", 0 } }, 1);
                TrainSet ts3 = new(new Dictionary<string, double> { { "I1", 0 }, { "I2", 1 } }, 1);
                TrainSet ts4 = new(new Dictionary<string, double> { { "I1", 1 }, { "I2", 1 } }, 1);

                return new Epoch(new List<TrainSet> { ts1, ts2, ts3, ts4 });
            }
        }
    }
}
