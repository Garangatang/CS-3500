
namespace SS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cellContents = new System.Windows.Forms.TextBox();
            this.cellName = new System.Windows.Forms.Label();
            this.cellValue = new System.Windows.Forms.Label();
            this.setButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.originalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cellContents
            // 
            this.cellContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cellContents.Location = new System.Drawing.Point(355, 59);
            this.cellContents.Name = "cellContents";
            this.cellContents.Size = new System.Drawing.Size(154, 44);
            this.cellContents.TabIndex = 3;
            // 
            // cellName
            // 
            this.cellName.AutoSize = true;
            this.cellName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cellName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.cellName.Location = new System.Drawing.Point(251, 62);
            this.cellName.Name = "cellName";
            this.cellName.Size = new System.Drawing.Size(59, 39);
            this.cellName.TabIndex = 5;
            this.cellName.Text = "A1";
            this.cellName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cellValue
            // 
            this.cellValue.AutoSize = true;
            this.cellValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cellValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cellValue.Location = new System.Drawing.Point(718, 62);
            this.cellValue.Name = "cellValue";
            this.cellValue.Size = new System.Drawing.Size(119, 39);
            this.cellValue.TabIndex = 6;
            this.cellValue.Text = "Value: ";
            // 
            // setButton
            // 
            this.setButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.setButton.Location = new System.Drawing.Point(515, 57);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(134, 48);
            this.setButton.TabIndex = 7;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileDropDown,
            this.toolStripDropDownButton1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1865, 42);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileDropDown
            // 
            this.fileDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStrip,
            this.closeToolStripMenuItem,
            this.saveToolStrip,
            this.openToolStrip});
            this.fileDropDown.Name = "fileDropDown";
            this.fileDropDown.Size = new System.Drawing.Size(74, 36);
            this.fileDropDown.Text = "File";
            // 
            // newToolStrip
            // 
            this.newToolStrip.Name = "newToolStrip";
            this.newToolStrip.Size = new System.Drawing.Size(208, 44);
            this.newToolStrip.Text = "New";
            this.newToolStrip.Click += new System.EventHandler(this.newToolStrip_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(208, 44);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveToolStrip
            // 
            this.saveToolStrip.Name = "saveToolStrip";
            this.saveToolStrip.Size = new System.Drawing.Size(208, 44);
            this.saveToolStrip.Text = "Save";
            this.saveToolStrip.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // openToolStrip
            // 
            this.openToolStrip.Name = "openToolStrip";
            this.openToolStrip.Size = new System.Drawing.Size(208, 44);
            this.openToolStrip.Text = "Open";
            this.openToolStrip.Click += new System.EventHandler(this.openToolStrip_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.originalToolStripMenuItem,
            this.redToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.pinkToolStripMenuItem,
            this.orangeToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(94, 36);
            this.toolStripDropDownButton1.Text = "Color";
            // 
            // originalToolStripMenuItem
            // 
            this.originalToolStripMenuItem.Name = "originalToolStripMenuItem";
            this.originalToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.originalToolStripMenuItem.Text = "Original";
            this.originalToolStripMenuItem.Click += new System.EventHandler(this.originalToolStripMenuItem_Click);
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.redToolStripMenuItem.Text = "Red";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.blueToolStripMenuItem.Text = "Blue";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.greenToolStripMenuItem.Text = "Green";
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
            // 
            // pinkToolStripMenuItem
            // 
            this.pinkToolStripMenuItem.Name = "pinkToolStripMenuItem";
            this.pinkToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.pinkToolStripMenuItem.Text = "Pink";
            this.pinkToolStripMenuItem.Click += new System.EventHandler(this.pinkToolStripMenuItem_Click);
            // 
            // orangeToolStripMenuItem
            // 
            this.orangeToolStripMenuItem.Name = "orangeToolStripMenuItem";
            this.orangeToolStripMenuItem.Size = new System.Drawing.Size(233, 44);
            this.orangeToolStripMenuItem.Text = "Orange";
            this.orangeToolStripMenuItem.Click += new System.EventHandler(this.orangeToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(69, 36);
            this.toolStripButton1.Text = "Help";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.BackColor = System.Drawing.Color.Transparent;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(1, 147);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1864, 765);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1865, 1023);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.cellValue);
            this.Controls.Add(this.cellName);
            this.Controls.Add(this.cellContents);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox cellContents;
        private System.Windows.Forms.Label cellName;
        private System.Windows.Forms.Label cellValue;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileDropDown;
        private System.Windows.Forms.ToolStripMenuItem newToolStrip;
        private System.Windows.Forms.ToolStripMenuItem saveToolStrip;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originalToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

