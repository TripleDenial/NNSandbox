using System;
using System.Threading;
using System.Windows.Forms;
using NNSandbox.Architecture;
using NNSandbox.Networks;
using NNSandbox.TrainSets;

namespace NNSandbox {
    public partial class NeuralNetworkSandbox : Form {
        private NeuralNetwork network;
        private readonly Epoch epoch;
        private bool stopped = true;
        private bool formCloseRequested;
        private CancellationTokenSource tokenSource;

        public NeuralNetworkSandbox() {
            InitializeComponent();
            NormalizeFunctionComboBox.Items.AddRange(ActivationFunction.Collection);
            ApplyInitialSettings();
            epoch = LolGames.GetEpoch(14);
        }

        private void ApplyInitialSettings() {
            NormalizeFunctionComboBox.SelectedIndex = 2;
            MomentumTextBox.Text = "0,3";
            LearningSpeedTextBox.Text = "0,7";
            PrecisionTextBox.Text = "0,48";
            ResetNetwork();
        }

        private void ResetNetwork() {
            network = Lol1.Create(NormalizeFunctionComboBox.SelectedItem as IActivationFunction);
            network.Momentum = double.Parse(MomentumTextBox.Text);
            network.LearningSpeed = double.Parse(LearningSpeedTextBox.Text);
            network.Precision = double.Parse(PrecisionTextBox.Text);
            network.Report = UpdateReport;
            NetworkTextBox.Text = network.ArchitectureAsText;
        }

        private void UpdateButtonState() {
            RunButton.Enabled = stopped;
            StopButton.Enabled = !stopped;
            TestButton.Enabled = stopped;
            ResetButton.Enabled = stopped;
        }

        private void Train(object parameter) {
            CancellationToken token = (CancellationToken)parameter;
            network.Validate();
            int epochCount = 0;
            string successEpoch = string.Empty;
            while (!token.IsCancellationRequested) {
                (double loss, double accuracy) = network.RunEpoch(epoch, token);
                ResultDataPoint result = new(++epochCount, accuracy, loss);
                epochCount++;
                if(Chart.FirstSuccess == null && accuracy == 1) {
                    Chart.FirstSuccess = result;
                    successEpoch = $"{Environment.NewLine}Success epoch: {successEpoch}";
                }
                //if (epochCount % 10 == 0)
                Chart.AddPoint(result);
            }
            if(formCloseRequested) {
                Invoke(Close);
            }
        }

        private void Test(object parameter) {
            CancellationToken token = (CancellationToken)parameter;
            network.Validate();
            double accuracy = network.TestEpoch(epoch, token);
            void action() { AccuracyTextBox.Text = accuracy.ToString("0.###"); }

            Invoke(action);
            if (formCloseRequested) {
                Invoke(Close);
            }
        }

        private void UpdateReport(int epochs, int sets, double loss, double accuracy) {
            if(InvokeRequired) {
                Invoke(UpdateReport, epochs, sets, loss, accuracy);
                return;
            }

            EpochCountTextBox.Text = epochs.ToString();
            SetCountTextBox.Text = sets.ToString();
            LossTextBox.Text = loss.ToString("0.###");
            AccuracyTextBox.Text = accuracy.ToString("0.###");
        }

        private void ResetCancellationSource() {
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
        }

        private void RunButton_Click(object sender, EventArgs e) {
            try {
                ResetCancellationSource();
                stopped = false;
                UpdateButtonState();
                Thread learningThread = new(Train);
                learningThread.Start(tokenSource.Token);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopButton_Click(object sender, EventArgs e) {
            tokenSource.Cancel();
            stopped = true;
            UpdateButtonState();
        }

        private void ResetButton_Click(object sender, EventArgs e) {
            try {
                NetworkTextBox.Clear();
                EpochCountTextBox.Clear();
                SetCountTextBox.Clear();
                LossTextBox.Clear();
                AccuracyTextBox.Clear();
                Chart.Flush();
                ResetNetwork();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void TestButton_Click(object sender, EventArgs e) {
            try {
                ResetCancellationSource();
                stopped = false;
                UpdateButtonState();
                Thread testingThread = new(Test);
                testingThread.Start(tokenSource.Token);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void NeuralNetworkSandbox_FormClosing(object sender, FormClosingEventArgs e) {
            if(!stopped) {
                formCloseRequested = true;
                StopButton_Click(sender, e);
                e.Cancel = true;
            }
        }
    }
}
