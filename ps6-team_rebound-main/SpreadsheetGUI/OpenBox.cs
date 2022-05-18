using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Class for opening a file when open button is pressed.
    /// </summary>
    public partial class OpenBox : Form
    {
        private bool onlySprdFiles;
        private bool allFiles;
        public OpenBox()
        {
            InitializeComponent();
            onlySprdFiles = false;
            allFiles = false;
        }

        /// <summary>
        /// Helper method to say only .sprd files are wanted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onlySprdButton_Click(object sender, EventArgs e)
        {
            onlySprdFiles = true;
            Close();
        }

        /// <summary>
        /// Helper method to say not only .sprd are wanted to be shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allFilesButton_Click(object sender, EventArgs e)
        {
            allFiles = true;
            Close();
        }

        public bool getOnlySprdFiles()
        {
            return onlySprdFiles;
        }
        public bool getAllFiles()
        {
            return allFiles;
        }

    }
}
