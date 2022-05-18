// Created by Haydn Thurman and Grange Simpson October 2021 for 
// PS6

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SpreadsheetGUI;

namespace SS
{
    /// <summary>
    /// Example of using a SpreadsheetPanel object
    /// </summary>
    public partial class Form1 : Form
    {
        string cN = "A1";
        AbstractSpreadsheet spreadsheet;
        int row =0;
        int col =0;

        /// <summary>
        /// Constructor for the demo
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            spreadsheet = new Spreadsheet(s => true, s=> s.ToUpper(), "ps6") ;

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
        }

        public Form1(string filepath)
        {
            InitializeComponent();

            spreadsheet = new Spreadsheet(filepath, s => true, s => s.ToUpper(), "ps6");
            putDataInOpenFile();

            cellName.Text = cN;
            cellContents.Text = spreadsheet.GetCellContents(cN).ToString();

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
        }

        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter.  We display the current time in the cell.
        private void displaySelection(SpreadsheetPanel ss)
        {
            ss.GetSelection(out col, out row);

            char letterOfCell = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[col];
            int rowNum = row + 1;
            cN = letterOfCell + rowNum.ToString();

            cellName.Text = cN;

            cellContents.Text = spreadsheet.GetCellContents(cN).ToString();

            if (spreadsheet.GetCellValue(cN) is SpreadsheetUtilities.FormulaError e)
            {
                cellValue.Text = "Value: " + e.Reason;
            }
            else
            {
                cellValue.Text = "Value: " + spreadsheet.GetCellValue(cN).ToString();
            }
        }

        /// <summary>
        /// Overriding red x on form to ensure a changed
        /// spreadsheet is not closed without saving if wanted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) 
                return;

            // Confirm user wants to close
            if (spreadsheet.Changed)
            {
                switch (MessageBox.Show(this, "Are you sure you want to close without saving?", "Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }

            else
            {
                return;
            }
        }

        /// <summary>
        /// Button to set the contents and value of a cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setButton_Click(object sender, EventArgs e)
        {
            updateValue();
        }

        /// <summary>
        /// Helper method to update all values for cells
        /// which are associated with a given cell if needed.
        /// </summary>
        private void updateValue()
        {
            try
            {
                spreadsheet.SetContentsOfCell(cN, cellContents.Text);
                string cellVal = spreadsheet.GetCellValue(cN).ToString();
                cellValue.Text = "Value: " + cellVal;
                spreadsheetPanel1.SetValue(col, row, cellVal);

                foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells() )
                {

                    int colCell = getCol(s[0]);
                    int rowCell = int.Parse(s[1].ToString()) - 1;

                    if (!(spreadsheet.GetCellValue(s) is SpreadsheetUtilities.FormulaError e))
                    {
                        spreadsheetPanel1.SetValue(colCell, rowCell, spreadsheet.GetCellValue(s).ToString());
                        cellValue.Text = "Value: " + spreadsheet.GetCellValue(s).ToString();
                    }
                    else
                    {
                        spreadsheetPanel1.SetValue(colCell, rowCell, e.Reason);
                        cellValue.Text = "Value: " + e.Reason;
                    }
                }
            }
            catch (Exception e)
            {
                string cellVal = spreadsheet.GetCellValue(cN).ToString();
                MessageBox.Show("There was an error: " + e.Message);
            }
        }

        /// <summary>
        /// Helper method to get the col selected on the spreadsheet.
        /// </summary>
        /// <param name="varLetter"></param>
        /// <returns></returns>
        private int getCol(char varLetter)
        {
            char norm = Char.ToUpper(varLetter);
            int num = (int)norm;
            return num - 65;
        }

        /// <summary>
        /// Method associated with creating a new spreadsheet in toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStrip_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// Method associated with closing spreadsheet in toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spreadsheet.Changed)
            {
                SavedWarning saveWarning = new SavedWarning();
                saveWarning.ShowDialog();

                if (saveWarning.getContinuePressed())
                {
                    Close();
                }
                if (saveWarning.getSavePressed())
                {
                    saveDialog();
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Method associated with saving spreadsheet in toolstrip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStrip_Click(object sender, EventArgs e)
        {
            saveDialog();
        }

        /// <summary>
        /// Helper method to save contents of spreadsheet.
        /// </summary>
        private void saveDialog()
        {
            try
            {
                string saveFileName = "ps6.sprd";

                SaveTextBox saveTB = new SaveTextBox();
                saveTB.ShowDialog();

                saveFileName = saveTB.getSaveName();

                if (!(Regex.IsMatch(saveFileName, ".sprd")))
                {
                    saveFileName += ".sprd";
                }

                spreadsheet.Save(saveFileName);

            }
            catch (Exception x)
            {
                MessageBox.Show("There was an error: " + x.Message);
            }
        }

        /// <summary>
        /// Method associated with opening a selected spreadsheet in the 
        /// toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStrip_Click(object sender, EventArgs e)
        {
            try
            {
                OpenBox openBox = new OpenBox();
                openBox.ShowDialog();

                if (openBox.getOnlySprdFiles())
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = Directory.GetCurrentDirectory();
                    ofd.Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";
                    ofd.ShowDialog();

                    SpreadsheetApplicationContext.getAppContext().RunForm(new Form1(ofd.FileName));
                }
                if (openBox.getAllFiles())
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = Directory.GetCurrentDirectory();
                    ofd.ShowDialog();

                    SpreadsheetApplicationContext.getAppContext().RunForm(new Form1(ofd.FileName));
                }
            }
            catch (Exception z)
            {
                MessageBox.Show("There was an error: " + z.Message);
            }

        }

        /// <summary>
        /// Method to put saved data into file being opened from toolbar.
        /// </summary>
        private void putDataInOpenFile()
        {
            try
            {

                
                foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    int colCell = getCol(s[0]);
                    int rowCell = int.Parse(s[1].ToString()) - 1;
                    spreadsheetPanel1.SetValue(colCell, rowCell, spreadsheet.GetCellValue(s).ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error: " + e.Message);
            }
        }

        /// <summary>
        /// Method associated with pressing help button in toolbar,
        /// will show a new helpBox outlining helpful tips.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpBox helpBox = new HelpBox();
            SpreadsheetApplicationContext.getAppContext().RunForm(helpBox);
        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// color to red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.Red;
        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// color to blue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.Blue;

        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// color to green.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.Green;

        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// color to pink.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.HotPink;

        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// color to orange.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.Orange;

        }

        /// <summary>
        /// Associated with button in colors toolbar to change spreadsheet
        /// to default color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Method associated with opening a help box from the toolstrip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            HelpBox helpBox = new HelpBox();
            SpreadsheetApplicationContext.getAppContext().RunForm(helpBox);
        }

    }
}

