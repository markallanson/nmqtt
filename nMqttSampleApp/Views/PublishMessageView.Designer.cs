/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

namespace nMqtt.SampleApp.Views
{
    partial class PublishMessageView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.publishGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextbox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.publishButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.qosNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.topicsCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.publishGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qosNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // publishGroupBox
            // 
            this.publishGroupBox.Controls.Add(this.messageTextbox);
            this.publishGroupBox.Controls.Add(this.button3);
            this.publishGroupBox.Controls.Add(this.button2);
            this.publishGroupBox.Controls.Add(this.publishButton);
            this.publishGroupBox.Controls.Add(this.checkBox1);
            this.publishGroupBox.Controls.Add(this.qosNumeric);
            this.publishGroupBox.Controls.Add(this.label2);
            this.publishGroupBox.Controls.Add(this.topicsCombo);
            this.publishGroupBox.Controls.Add(this.label1);
            this.publishGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publishGroupBox.Location = new System.Drawing.Point(0, 0);
            this.publishGroupBox.Name = "publishGroupBox";
            this.publishGroupBox.Size = new System.Drawing.Size(478, 282);
            this.publishGroupBox.TabIndex = 0;
            this.publishGroupBox.TabStop = false;
            this.publishGroupBox.Text = "Publishing";
            // 
            // textBox1
            // 
            this.messageTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextbox.Location = new System.Drawing.Point(11, 51);
            this.messageTextbox.Multiline = true;
            this.messageTextbox.Name = "messageTextbox";
            this.messageTextbox.Size = new System.Drawing.Size(380, 225);
            this.messageTextbox.TabIndex = 22;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(397, 111);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Hex";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(397, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "File...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.publishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.publishButton.Location = new System.Drawing.Point(397, 51);
            this.publishButton.Name = "publishButton";
            this.publishButton.Size = new System.Drawing.Size(75, 23);
            this.publishButton.TabIndex = 19;
            this.publishButton.Text = "Publish";
            this.publishButton.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(403, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Retained";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.qosNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qosNumeric.Location = new System.Drawing.Point(364, 20);
            this.qosNumeric.Name = "qosNumeric";
            this.qosNumeric.Size = new System.Drawing.Size(31, 20);
            this.qosNumeric.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "At Qos:";
            // 
            // comboBox1
            // 
            this.topicsCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topicsCombo.FormattingEnabled = true;
            this.topicsCombo.Location = new System.Drawing.Point(88, 19);
            this.topicsCombo.Name = "topicsCombo";
            this.topicsCombo.Size = new System.Drawing.Size(233, 21);
            this.topicsCombo.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Publish Topic:";
            // 
            // PublishMessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.publishGroupBox);
            this.Name = "PublishMessageView";
            this.Size = new System.Drawing.Size(478, 282);
            this.publishGroupBox.ResumeLayout(false);
            this.publishGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qosNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox publishGroupBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown qosNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox topicsCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox messageTextbox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button publishButton;
    }
}
