// Created by Haydn Thurman and Grange Simpson October 2021 for 
// PS6

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
    /// Method associated with save warning.
    /// Shows a warning dialogue if a spreadsheet is going to be
    /// closed with data that has not been saved.
    /// </summary>
    public partial class SavedWarning : Form
    {
        private bool continuePressed;
        private bool savePressed;

        public SavedWarning()
        {
            InitializeComponent();
            continuePressed = false;
            savePressed = false;
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            continuePressed = true;
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            savePressed = true;
            Close();
        }

        public bool getContinuePressed()
        {
            return continuePressed;
        }

        public bool getSavePressed()
        {
            return savePressed;
        }
    }
}
