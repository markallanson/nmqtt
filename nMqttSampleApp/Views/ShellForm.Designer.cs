/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

namespace nMqtt.SampleApp.Views
{
    partial class ShellForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.nmqttTabPage = new System.Windows.Forms.TabPage();
            this.optionsTabPage = new System.Windows.Forms.TabPage();
            this.publishMessageView1 = new nMqtt.SampleApp.Views.PublishMessageView();
            this.subscriptionView1 = new nMqtt.SampleApp.Views.SubscriptionView();
            this.connectionView1 = new nMqtt.SampleApp.Views.ConnectionView();
            this.tabControl.SuspendLayout();
            this.nmqttTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.nmqttTabPage);
            this.tabControl.Controls.Add(this.optionsTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(477, 542);
            this.tabControl.TabIndex = 0;
            // 
            // nmqttTabPage
            // 
            this.nmqttTabPage.Controls.Add(this.publishMessageView1);
            this.nmqttTabPage.Controls.Add(this.subscriptionView1);
            this.nmqttTabPage.Controls.Add(this.connectionView1);
            this.nmqttTabPage.Location = new System.Drawing.Point(4, 22);
            this.nmqttTabPage.Name = "nmqttTabPage";
            this.nmqttTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.nmqttTabPage.Size = new System.Drawing.Size(469, 516);
            this.nmqttTabPage.TabIndex = 0;
            this.nmqttTabPage.Text = "nMqtt";
            this.nmqttTabPage.UseVisualStyleBackColor = true;
            // 
            // optionsTabPage
            // 
            this.optionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.optionsTabPage.Name = "optionsTabPage";
            this.optionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.optionsTabPage.Size = new System.Drawing.Size(469, 516);
            this.optionsTabPage.TabIndex = 1;
            this.optionsTabPage.Text = "Options";
            this.optionsTabPage.UseVisualStyleBackColor = true;
            // 
            // publishMessageView1
            // 
            this.publishMessageView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.publishMessageView1.Location = new System.Drawing.Point(3, 310);
            this.publishMessageView1.Name = "publishMessageView1";
            this.publishMessageView1.Size = new System.Drawing.Size(463, 203);
            this.publishMessageView1.TabIndex = 2;
            // 
            // subscriptionView1
            // 
            this.subscriptionView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subscriptionView1.Location = new System.Drawing.Point(3, 85);
            this.subscriptionView1.Name = "subscriptionView1";
            this.subscriptionView1.Size = new System.Drawing.Size(463, 219);
            this.subscriptionView1.TabIndex = 1;
            // 
            // connectionView1
            // 
            this.connectionView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectionView1.Location = new System.Drawing.Point(3, 3);
            this.connectionView1.Name = "connectionView1";
            this.connectionView1.Size = new System.Drawing.Size(463, 76);
            this.connectionView1.TabIndex = 0;
            // 
            // ShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 542);
            this.Controls.Add(this.tabControl);
            this.Name = "ShellForm";
            this.Text = "nMqtt Utility";
            this.tabControl.ResumeLayout(false);
            this.nmqttTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage nmqttTabPage;
        private System.Windows.Forms.TabPage optionsTabPage;
        private ConnectionView connectionView1;
        private SubscriptionView subscriptionView1;
        private PublishMessageView publishMessageView1;
    }
}