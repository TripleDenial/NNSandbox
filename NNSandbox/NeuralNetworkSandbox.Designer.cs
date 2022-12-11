﻿
namespace NeuralNetworkSandbox {
    partial class NeuralNetworkSandbox {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.RunButton = new System.Windows.Forms.Button();
            this.NetworkTextBox = new System.Windows.Forms.TextBox();
            this.Chart = new global::NeuralNetworkSandbox.Chart();
            this.StopButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MomentumLabel = new System.Windows.Forms.Label();
            this.MomentumTextBox = new System.Windows.Forms.TextBox();
            this.LearningSpeedLabel = new System.Windows.Forms.Label();
            this.LearningSpeedTextBox = new System.Windows.Forms.TextBox();
            this.NormalizeFunctionComboBox = new System.Windows.Forms.ComboBox();
            this.PrecisionTextBox = new System.Windows.Forms.TextBox();
            this.PrecisionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(12, 12);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // NetworkTextBox
            // 
            this.NetworkTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NetworkTextBox.Location = new System.Drawing.Point(0, 0);
            this.NetworkTextBox.Multiline = true;
            this.NetworkTextBox.Name = "NetworkTextBox";
            this.NetworkTextBox.ReadOnly = true;
            this.NetworkTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.NetworkTextBox.Size = new System.Drawing.Size(370, 263);
            this.NetworkTextBox.TabIndex = 1;
            // 
            // Chart
            // 
            this.Chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Chart.BackColor = System.Drawing.Color.White;
            this.Chart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chart.FirstSuccess = null;
            this.Chart.Location = new System.Drawing.Point(388, 12);
            this.Chart.Name = "Chart";
            this.Chart.Size = new System.Drawing.Size(935, 613);
            this.Chart.TabIndex = 2;
            this.Chart.TabStop = false;
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(93, 12);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 3;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(307, 12);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultTextBox.Location = new System.Drawing.Point(0, 0);
            this.ResultTextBox.Multiline = true;
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ReadOnly = true;
            this.ResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultTextBox.Size = new System.Drawing.Size(370, 259);
            this.ResultTextBox.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 99);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.NetworkTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ResultTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(370, 526);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 6;
            // 
            // MomentumLabel
            // 
            this.MomentumLabel.AutoSize = true;
            this.MomentumLabel.Location = new System.Drawing.Point(155, 73);
            this.MomentumLabel.Name = "MomentumLabel";
            this.MomentumLabel.Size = new System.Drawing.Size(71, 15);
            this.MomentumLabel.TabIndex = 9;
            this.MomentumLabel.Text = "Momentum";
            // 
            // MomentumTextBox
            // 
            this.MomentumTextBox.Location = new System.Drawing.Point(232, 70);
            this.MomentumTextBox.Name = "MomentumTextBox";
            this.MomentumTextBox.Size = new System.Drawing.Size(43, 23);
            this.MomentumTextBox.TabIndex = 7;
            // 
            // LearningSpeedLabel
            // 
            this.LearningSpeedLabel.AutoSize = true;
            this.LearningSpeedLabel.Location = new System.Drawing.Point(12, 73);
            this.LearningSpeedLabel.Name = "LearningSpeedLabel";
            this.LearningSpeedLabel.Size = new System.Drawing.Size(87, 15);
            this.LearningSpeedLabel.TabIndex = 10;
            this.LearningSpeedLabel.Text = "Learning speed";
            // 
            // LearningSpeedTextBox
            // 
            this.LearningSpeedTextBox.Location = new System.Drawing.Point(105, 70);
            this.LearningSpeedTextBox.Name = "LearningSpeedTextBox";
            this.LearningSpeedTextBox.Size = new System.Drawing.Size(44, 23);
            this.LearningSpeedTextBox.TabIndex = 8;
            // 
            // NormalizeFunctionComboBox
            // 
            this.NormalizeFunctionComboBox.Location = new System.Drawing.Point(12, 41);
            this.NormalizeFunctionComboBox.Name = "NormalizeFunctionComboBox";
            this.NormalizeFunctionComboBox.Size = new System.Drawing.Size(370, 23);
            this.NormalizeFunctionComboBox.TabIndex = 11;
            // 
            // PrecisionTextBox
            // 
            this.PrecisionTextBox.Location = new System.Drawing.Point(339, 70);
            this.PrecisionTextBox.Name = "PrecisionTextBox";
            this.PrecisionTextBox.Size = new System.Drawing.Size(43, 23);
            this.PrecisionTextBox.TabIndex = 12;
            // 
            // PrecisionLabel
            // 
            this.PrecisionLabel.AutoSize = true;
            this.PrecisionLabel.Location = new System.Drawing.Point(281, 73);
            this.PrecisionLabel.Name = "PrecisionLabel";
            this.PrecisionLabel.Size = new System.Drawing.Size(52, 15);
            this.PrecisionLabel.TabIndex = 13;
            this.PrecisionLabel.Text = "Precison";
            // 
            // NeuralNetworkSandbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 637);
            this.Controls.Add(this.PrecisionLabel);
            this.Controls.Add(this.PrecisionTextBox);
            this.Controls.Add(this.LearningSpeedTextBox);
            this.Controls.Add(this.MomentumTextBox);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.Chart);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.MomentumLabel);
            this.Controls.Add(this.LearningSpeedLabel);
            this.Controls.Add(this.NormalizeFunctionComboBox);
            this.Name = "NeuralNetworkSandbox";
            this.Text = "NeuralNetworkSandbox";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.TextBox NetworkTextBox;
        private Chart Chart;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox MomentumTextBox;
        private System.Windows.Forms.TextBox LearningSpeedTextBox;
        private System.Windows.Forms.Label MomentumLabel;
        private System.Windows.Forms.Label LearningSpeedLabel;
        private System.Windows.Forms.ComboBox NormalizeFunctionComboBox;
        private System.Windows.Forms.TextBox PrecisionTextBox;
        private System.Windows.Forms.Label PrecisionLabel;
    }
}

