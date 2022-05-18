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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Class for designating the file name when saving the spreadsheet
    /// into a file.
    /// </summary>
    public partial class SaveTextBox : Form
    {
        string saveName;
        bool reWrite;
        bool fileMatch;
        public SaveTextBox()
        {
            InitializeComponent();
            saveName = "ps6.sprd";
            reWrite = false;
            fileMatch = false;
        }

        public string getSaveName()
        {
            saveName = saveNameTextBox.Text;
            return saveName;
        }

        private void saveButton_Click_1(object sender, EventArgs e)
        {

            saveName = saveNameTextBox.Text;
            List<string> files = Directory.GetFiles(path: Directory.GetCurrentDirectory(), "*.sprd", SearchOption.AllDirectories).ToList();
            foreach (string file in files)
            {
                if (Regex.IsMatch(file, saveName))
                {
                    fileMatch = true;
                    WarningDialog warningDialog = new WarningDialog();
                    warningDialog.ShowDialog();
                    if (warningDialog.getYesClicked())
                    {
                        reWrite = true;
                    }
                    if (warningDialog.getNoClicked())
                    {
                        reWrite = false;
                    }
                }
            }
            if (fileMatch)
            {
                if (reWrite)
                {
                    Close();
                }
                if (!reWrite)
                {
                    fileMatch = false;
                }
            }
            else
            {
                Close();
            }
        
        }
        public bool getReWrite()
        {
            return reWrite;
        }
    }



}
