using System;
using System.Threading;
using System.Windows.Forms;
using NNSandbox.Architecture;
using NNSandbox.Networks;
using NNSandbox.TrainSets;

namespace NNSandbox {
    public partial class NeuralNetworkSandbox : Form {
        private NeuralNetwork network;
        private bool stopped = true;

        public NeuralNetworkSandbox() {
            InitializeComponent();
            NormalizeFunctionComboBox.Items.AddRange(ActivationFunction.Collection);
            ApplyInitialSettings();
        }

        private void ApplyInitialSettings() {
            NormalizeFunctionComboBox.SelectedIndex = 2;
            MomentumTextBox.Text = "0,3";
            LearningSpeedTextBox.Text = "0,3";
            PrecisionTextBox.Text = "0,1";
            ResetNetwork();
        }

        private void UpdateButtonState() {
            RunButton.Enabled = stopped;
            StopButton.Enabled = !stopped;
            ResetButton.Enabled = stopped;
        }

        private void ResetNetwork() {
            network = Lol1.Create(NormalizeFunctionComboBox.SelectedItem as IActivationFunction);
            network.Momentum = double.Parse(MomentumTextBox.Text);
            network.LearningSpeed = double.Parse(LearningSpeedTextBox.Text);
            network.Precision = double.Parse(PrecisionTextBox.Text);
            network.Log = ShowText;
            NetworkTextBox.Text = network.ArchitectureAsText;
        }

        private void Train() {
            int epochCount = 0;
            string successEpoch = string.Empty;
            while (!stopped) {
                (double loss, double accuracy) = network.RunEpoch(LolGames.GetEpoch(14));
                ResultDataPoint result = new(++epochCount, accuracy, loss);
                epochCount++;
                if(Chart.FirstSuccess == null && accuracy == 1) {
                    Chart.FirstSuccess = result;
                    successEpoch = $"{Environment.NewLine}Success epoch: {successEpoch}";
                }
                //if (epochCount % 10 == 0)
                    Chart.AddPoint(result);
                ShowText($"Epochs: {epochCount}{Environment.NewLine}Accuracy: {accuracy:0.###}{Environment.NewLine}Loss(MSE): {loss:0.###}{successEpoch}");
            }
        }

        private void ShowText(string str) {
            if(ResultTextBox.InvokeRequired) {
                void action() { ShowText(str); }
                ResultTextBox?.Invoke(action);
                return;
            }
            ResultTextBox.Text = str;
        }

        private void RunButton_Click(object sender, EventArgs e) {
            try {
                stopped = false;
                UpdateButtonState();
                Thread learningThread = new(Train);
                learningThread.Start();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopButton_Click(object sender, EventArgs e) {
            stopped = true;
            UpdateButtonState();
        }

        private void ResetButton_Click(object sender, EventArgs e) {
            try {
                NetworkTextBox.Clear();
                ResultTextBox.Clear();
                Chart.Flush();
                ResetNetwork();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
