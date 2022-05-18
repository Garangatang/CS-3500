
namespace SpreadsheetGUI
{
    partial class OpenBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.onlySprdButton = new System.Windows.Forms.Button();
            this.allFilesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(155, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "What would you like to open?";
            // 
            // onlySprdButton
            // 
            this.onlySprdButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.onlySprdButton.Location = new System.Drawing.Point(209, 160);
            this.onlySprdButton.Name = "onlySprdButton";
            this.onlySprdButton.Size = new System.Drawing.Size(319, 53);
            this.onlySprdButton.TabIndex = 1;
            this.onlySprdButton.Text = "Only spreadsheet files";
            this.onlySprdButton.UseVisualStyleBackColor = true;
            this.onlySprdButton.Click += new System.EventHandler(this.onlySprdButton_Click);
            // 
            // allFilesButton
            // 
            this.allFilesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.allFilesButton.Location = new System.Drawing.Point(209, 243);
            this.allFilesButton.Name = "allFilesButton";
            this.allFilesButton.Size = new System.Drawing.Size(319, 53);
            this.allFilesButton.TabIndex = 2;
            this.allFilesButton.Text = "All files";
            this.allFilesButton.UseVisualStyleBackColor = true;
            this.allFilesButton.Click += new System.EventHandler(this.allFilesButton_Click);
            // 
            // OpenBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.allFilesButton);
            this.Controls.Add(this.onlySprdButton);
            this.Controls.Add(this.label1);
            this.Name = "OpenBox";
            this.Text = "OpenBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button onlySprdButton;
        private System.Windows.Forms.Button allFilesButton;
    }
}