using System;

namespace NeuralNetworkSandbox.Architecture {

    public static class ActivationFunction {
        public static IActivationFunction[] Collection { get; }
        static ActivationFunction() {
            Collection = new IActivationFunction[] {
                new ConstantOne(),
                new Linear(),
                new Sigmoid(),
                new HyperbolicTangens(),
                new Relu()
            };
        }
    }

    public interface IActivationFunction {
        public double CalcFunction(double x);

        public double CalcDerivative(double x);
    }

    public class ConstantOne : IActivationFunction {
        public double CalcFunction(double x) {
            return 1;
        }

        public double CalcDerivative(double x) {
            return 0;
        }
    }

    public class Linear : IActivationFunction {
        public double CalcFunction(double x) {
            return x;
        }

        public double CalcDerivative(double x) {
            return 1;
        }
    }

    public class Sigmoid : IActivationFunction {
        public double CalcFunction(double x) {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        public double CalcDerivative(double x) {
            return (1 - x) * x;
        }
    }

    public class HyperbolicTangens : IActivationFunction {
        public double CalcFunction(double x) {
            return (Math.Pow(Math.E, 2 * x) - 1) / (Math.Pow(Math.E, 2 * x) + 1);
        }

        public double CalcDerivative(double x) {
            return 1 - Math.Pow(x, 2);
        }
    }

    public class Relu : IActivationFunction {
        public double CalcFunction(double x) {
            return x > 0 ? x : 0;
        }

        public double CalcDerivative(double x) {
            return x > 0 ? 1 : 0;
        }
    }
}
