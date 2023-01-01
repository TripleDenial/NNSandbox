namespace NNSandbox {
    public class ResultDataPoint {
        public int EpochId { get; }

        public double Accuracy { get; }

        public double Loss { get; }

        public ResultDataPoint(int epochId, double accuracy, double loss) {
            EpochId = epochId;
            Accuracy = accuracy;
            Loss = loss;
        }
    }
}
