
namespace Illumine
{
    partial class MonitorSetter
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
            this.SetMonitorButton = new System.Windows.Forms.Button();
            this.CancelSetButton = new System.Windows.Forms.Button();
            this.PlaceWindowPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SetMonitorButton
            // 
            this.SetMonitorButton.Location = new System.Drawing.Point(70, 86);
            this.SetMonitorButton.Name = "SetMonitorButton";
            this.SetMonitorButton.Size = new System.Drawing.Size(75, 23);
            this.SetMonitorButton.TabIndex = 2;
            this.SetMonitorButton.Text = "Set";
            this.SetMonitorButton.UseVisualStyleBackColor = true;
            this.SetMonitorButton.Click += new System.EventHandler(this.SetMonitorButton_Click);
            // 
            // CancelSetButton
            // 
            this.CancelSetButton.Location = new System.Drawing.Point(180, 86);
            this.CancelSetButton.Name = "CancelSetButton";
            this.CancelSetButton.Size = new System.Drawing.Size(75, 23);
            this.CancelSetButton.TabIndex = 3;
            this.CancelSetButton.Text = "Cancel";
            this.CancelSetButton.UseVisualStyleBackColor = true;
            this.CancelSetButton.Click += new System.EventHandler(this.CancelSetButton_Click);
            // 
            // PlaceWindowPrompt
            // 
            this.PlaceWindowPrompt.AutoSize = true;
            this.PlaceWindowPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlaceWindowPrompt.Location = new System.Drawing.Point(21, 33);
            this.PlaceWindowPrompt.Name = "PlaceWindowPrompt";
            this.PlaceWindowPrompt.Size = new System.Drawing.Size(283, 20);
            this.PlaceWindowPrompt.TabIndex = 0;
            this.PlaceWindowPrompt.Text = "Place me on the monitor and press Set";
            this.PlaceWindowPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MonitorSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 121);
            this.ControlBox = false;
            this.Controls.Add(this.CancelSetButton);
            this.Controls.Add(this.SetMonitorButton);
            this.Controls.Add(this.PlaceWindowPrompt);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonitorSetter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Set Illumine Monitor";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MonitorSetter_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PlaceWindowPrompt;
        private System.Windows.Forms.Button SetMonitorButton;
        private System.Windows.Forms.Button CancelSetButton;
    }
}