using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CalcButton_Click(object sender, EventArgs e)
        {
            updateBill();
        }

        private void updateBill()
        {
            if (!hasValidInput())
            {
                return;
            }
            Double.TryParse(billBox.Text, out double bill);
            totalBox.Text = (bill + (bill * 0.4)).ToString();
            Double.TryParse(tipBox.Text, out double tip);
            totalBox.Text = (bill + bill * tip).ToString();
        }

        private bool hasValidInput()
        {
            if (!Double.TryParse(billBox.Text, out double doubleVal) ||
                !double.TryParse(tipBox.Text, out doubleVal))
            {
                CalcButton.Enabled = false;
                return false;
            }
            else
            {
                CalcButton.Enabled = true;
                return true;
            }
        }

        private void tipBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void totalBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
