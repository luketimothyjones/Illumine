
namespace Illumine
{
    partial class Searchbar
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
            this.FocusThiefLabel = new System.Windows.Forms.Label();
            this.SearchInput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // FocusThiefLabel
            // 
            this.FocusThiefLabel.AutoSize = true;
            this.FocusThiefLabel.Location = new System.Drawing.Point(12, 12);
            this.FocusThiefLabel.Name = "FocusThiefLabel";
            this.FocusThiefLabel.Size = new System.Drawing.Size(0, 13);
            this.FocusThiefLabel.TabIndex = 0;
            // 
            // SearchInput
            // 
            this.SearchInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.SearchInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SearchInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.SearchInput.ForeColor = System.Drawing.Color.White;
            this.SearchInput.Location = new System.Drawing.Point(10, 10);
            this.SearchInput.Margin = new System.Windows.Forms.Padding(20);
            this.SearchInput.MaximumSize = new System.Drawing.Size(690, 50);
            this.SearchInput.Multiline = false;
            this.SearchInput.Name = "SearchInput";
            this.SearchInput.Size = new System.Drawing.Size(680, 40);
            this.SearchInput.TabIndex = 1;
            this.SearchInput.Text = "";
            this.SearchInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchInput_KeyUp);
            // 
            // Searchbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(700, 60);
            this.Controls.Add(this.SearchInput);
            this.Controls.Add(this.FocusThiefLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 20);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Searchbar";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EverythingSearchbar";
            this.Deactivate += new System.EventHandler(this.Searchbar_Deactivate);
            this.Load += new System.EventHandler(this.Searchbar_Load);
            this.SizeChanged += new System.EventHandler(this.Searchbar_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.Searchbar_VisibleChanged);
            this.Click += new System.EventHandler(this.Searchbar_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label FocusThiefLabel;
        private System.Windows.Forms.RichTextBox SearchInput;
    }
}

