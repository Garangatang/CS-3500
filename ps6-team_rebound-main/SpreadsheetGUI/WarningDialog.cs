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
    public partial class WarningDialog : Form
    {
        private bool yesClicked;
        private bool noClicked;
        public WarningDialog()
        {
            yesClicked = false;
            noClicked = false;
            InitializeComponent();
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            yesClicked = true;
            Close();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            noClicked = true;
            Close();
        }

        public bool getYesClicked()
        {
            return yesClicked;
        }

        public bool getNoClicked()
        {
            return noClicked;
        }
    }
}
