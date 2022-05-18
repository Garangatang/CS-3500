
namespace Lab6
{
    partial class Form1
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
            this.calculateBill = new System.Windows.Forms.Label();
            this.CalcButton = new System.Windows.Forms.Button();
            this.billBox = new System.Windows.Forms.TextBox();
            this.totalBox = new System.Windows.Forms.TextBox();
            this.tipBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // calculateBill
            // 
            this.calculateBill.AutoSize = true;
            this.calculateBill.Location = new System.Drawing.Point(27, 23);
            this.calculateBill.Name = "calculateBill";
            this.calculateBill.Size = new System.Drawing.Size(137, 25);
            this.calculateBill.TabIndex = 0;
            this.calculateBill.Text = "Calculate Bill";
            // 
            // CalcButton
            // 
            this.CalcButton.Location = new System.Drawing.Point(12, 205);
            this.CalcButton.Name = "CalcButton";
            this.CalcButton.Size = new System.Drawing.Size(130, 41);
            this.CalcButton.TabIndex = 1;
            this.CalcButton.Text = "Calculate";
            this.CalcButton.UseVisualStyleBackColor = true;
            this.CalcButton.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // billBox
            // 
            this.billBox.Location = new System.Drawing.Point(313, 23);
            this.billBox.Name = "billBox";
            this.billBox.Size = new System.Drawing.Size(116, 31);
            this.billBox.TabIndex = 2;
            this.billBox.Text = "100";
            this.billBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // totalBox
            // 
            this.totalBox.Location = new System.Drawing.Point(313, 210);
            this.totalBox.Name = "totalBox";
            this.totalBox.Size = new System.Drawing.Size(116, 31);
            this.totalBox.TabIndex = 3;
            this.totalBox.Text = "0.5";
            this.totalBox.TextChanged += new System.EventHandler(this.totalBox_TextChanged);
            // 
            // tipBox
            // 
            this.tipBox.Location = new System.Drawing.Point(313, 115);
            this.tipBox.Name = "tipBox";
            this.tipBox.Size = new System.Drawing.Size(116, 31);
            this.tipBox.TabIndex = 4;
            this.tipBox.Text = "0.5";
            this.tipBox.TextChanged += new System.EventHandler(this.tipBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tipBox);
            this.Controls.Add(this.totalBox);
            this.Controls.Add(this.billBox);
            this.Controls.Add(this.CalcButton);
            this.Controls.Add(this.calculateBill);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label calculateBill;
        private System.Windows.Forms.Button CalcButton;
        private System.Windows.Forms.TextBox billBox;
        private System.Windows.Forms.TextBox totalBox;
        private System.Windows.Forms.TextBox tipBox;
    }
}

