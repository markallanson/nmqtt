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
    partial class SubscriptionView
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
            this.subscriptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.unsubscribeButton = new System.Windows.Forms.Button();
            this.subscribeButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.receivedTopicTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.qosNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.topicsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.messageHistory = new System.Windows.Forms.TextBox();
            this.subscriptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qosNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // subscriptionsGroupBox
            // 
            this.subscriptionsGroupBox.Controls.Add(this.messageHistory);
            this.subscriptionsGroupBox.Controls.Add(this.button4);
            this.subscriptionsGroupBox.Controls.Add(this.button3);
            this.subscriptionsGroupBox.Controls.Add(this.unsubscribeButton);
            this.subscriptionsGroupBox.Controls.Add(this.subscribeButton);
            this.subscriptionsGroupBox.Controls.Add(this.checkBox1);
            this.subscriptionsGroupBox.Controls.Add(this.textBox2);
            this.subscriptionsGroupBox.Controls.Add(this.label4);
            this.subscriptionsGroupBox.Controls.Add(this.receivedTopicTextbox);
            this.subscriptionsGroupBox.Controls.Add(this.label3);
            this.subscriptionsGroupBox.Controls.Add(this.qosNumeric);
            this.subscriptionsGroupBox.Controls.Add(this.label2);
            this.subscriptionsGroupBox.Controls.Add(this.topicsComboBox);
            this.subscriptionsGroupBox.Controls.Add(this.label1);
            this.subscriptionsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subscriptionsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.subscriptionsGroupBox.Name = "subscriptionsGroupBox";
            this.subscriptionsGroupBox.Size = new System.Drawing.Size(478, 282);
            this.subscriptionsGroupBox.TabIndex = 0;
            this.subscriptionsGroupBox.TabStop = false;
            this.subscriptionsGroupBox.Text = "Subscriptions";
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(393, 158);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "Hex";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(393, 129);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Save...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // unsubscribeButton
            // 
            this.unsubscribeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unsubscribeButton.Location = new System.Drawing.Point(393, 100);
            this.unsubscribeButton.Name = "unsubscribeButton";
            this.unsubscribeButton.Size = new System.Drawing.Size(75, 23);
            this.unsubscribeButton.TabIndex = 11;
            this.unsubscribeButton.Text = "Unsubscribe";
            this.unsubscribeButton.UseVisualStyleBackColor = true;
            // 
            // subscribeButton
            // 
            this.subscribeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subscribeButton.Location = new System.Drawing.Point(393, 71);
            this.subscribeButton.Name = "subscribeButton";
            this.subscribeButton.Size = new System.Drawing.Size(75, 23);
            this.subscribeButton.TabIndex = 10;
            this.subscribeButton.Text = "Subscribe";
            this.subscribeButton.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(396, 48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Retained";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(365, 45);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(22, 20);
            this.textBox2.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(330, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Qos:";
            // 
            // textBox1
            // 
            this.receivedTopicTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.receivedTopicTextbox.Location = new System.Drawing.Point(100, 45);
            this.receivedTopicTextbox.Name = "receivedTopicTextbox";
            this.receivedTopicTextbox.Size = new System.Drawing.Size(224, 20);
            this.receivedTopicTextbox.TabIndex = 5;            
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Received Topic:";
            // 
            // qosNumeric
            // 
            this.qosNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qosNumeric.Location = new System.Drawing.Point(437, 18);
            this.qosNumeric.Name = "qosNumeric";
            this.qosNumeric.Size = new System.Drawing.Size(31, 20);
            this.qosNumeric.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(393, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "At Qos:";
            // 
            // topicsComboBox
            // 
            this.topicsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topicsComboBox.FormattingEnabled = true;
            this.topicsComboBox.Location = new System.Drawing.Point(100, 17);
            this.topicsComboBox.Name = "topicsComboBox";
            this.topicsComboBox.Size = new System.Drawing.Size(287, 21);
            this.topicsComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subscribe Topic:";
            // 
            // textBox3
            // 
            this.messageHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageHistory.Location = new System.Drawing.Point(10, 71);
            this.messageHistory.Multiline = true;
            this.messageHistory.Name = "textBox3";
            this.messageHistory.Size = new System.Drawing.Size(377, 205);
            this.messageHistory.TabIndex = 14;
            // 
            // SubscriptionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subscriptionsGroupBox);
            this.Name = "SubscriptionView";
            this.Size = new System.Drawing.Size(478, 282);
            this.subscriptionsGroupBox.ResumeLayout(false);
            this.subscriptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qosNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox subscriptionsGroupBox;
        private System.Windows.Forms.NumericUpDown qosNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox topicsComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox receivedTopicTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button unsubscribeButton;
        private System.Windows.Forms.Button subscribeButton;
        private System.Windows.Forms.TextBox messageHistory;
    }
}
