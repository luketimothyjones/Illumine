
namespace Illumine
{
    partial class KeybindSetter
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
            System.Windows.Forms.Label PressKeysPrompt;
            this.KeysPressedLabel = new System.Windows.Forms.Label();
            this.SetKeybindButton = new System.Windows.Forms.Button();
            this.ResetKeybindButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            PressKeysPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PressKeysPrompt
            // 
            PressKeysPrompt.AutoSize = true;
            PressKeysPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PressKeysPrompt.Location = new System.Drawing.Point(80, 20);
            PressKeysPrompt.Name = "PressKeysPrompt";
            PressKeysPrompt.Size = new System.Drawing.Size(160, 26);
            PressKeysPrompt.TabIndex = 0;
            PressKeysPrompt.Text = "Press any keys";
            PressKeysPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KeysPressedLabel
            // 
            this.KeysPressedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeysPressedLabel.Location = new System.Drawing.Point(29, 46);
            this.KeysPressedLabel.Name = "KeysPressedLabel";
            this.KeysPressedLabel.Size = new System.Drawing.Size(263, 24);
            this.KeysPressedLabel.TabIndex = 1;
            this.KeysPressedLabel.Text = "...";
            this.KeysPressedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetKeybindButton
            // 
            this.SetKeybindButton.Location = new System.Drawing.Point(70, 86);
            this.SetKeybindButton.Name = "SetKeybindButton";
            this.SetKeybindButton.Size = new System.Drawing.Size(75, 23);
            this.SetKeybindButton.TabIndex = 2;
            this.SetKeybindButton.Text = "Set";
            this.SetKeybindButton.UseVisualStyleBackColor = true;
            this.SetKeybindButton.Click += new System.EventHandler(this.SetKeybindButton_Click);
            // 
            // ResetKeybindButton
            // 
            this.ResetKeybindButton.Location = new System.Drawing.Point(180, 86);
            this.ResetKeybindButton.Name = "ResetKeybindButton";
            this.ResetKeybindButton.Size = new System.Drawing.Size(75, 23);
            this.ResetKeybindButton.TabIndex = 3;
            this.ResetKeybindButton.Text = "Reset";
            this.ResetKeybindButton.UseVisualStyleBackColor = true;
            this.ResetKeybindButton.Click += new System.EventHandler(this.ResetKeybindButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.Location = new System.Drawing.Point(298, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(23, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "✕";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // KeybindSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 121);
            this.ControlBox = false;
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ResetKeybindButton);
            this.Controls.Add(this.SetKeybindButton);
            this.Controls.Add(this.KeysPressedLabel);
            this.Controls.Add(PressKeysPrompt);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeybindSetter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Set Illumine Keybind";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.KeybindSetter_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeybindSetter_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label KeysPressedLabel;
        private System.Windows.Forms.Button SetKeybindButton;
        private System.Windows.Forms.Button ResetKeybindButton;
        private System.Windows.Forms.Button CloseButton;
    }
}