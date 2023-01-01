﻿using System.Collections.Generic;

namespace NeuralNetworkSandbox.TrainSets {
    public class Epoch {
        private readonly List<TrainSet> trainSets;
        public List<TrainSet> TrainSets => trainSets;

        public Epoch(List<TrainSet> trainSets) {
            this.trainSets = trainSets;
        }
    }
}