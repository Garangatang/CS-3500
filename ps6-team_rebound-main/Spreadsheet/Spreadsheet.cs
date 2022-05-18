// Created by Grange Simpson in September 2021 for PS5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using SpreadsheetUtilities;
using SS;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> spreadSheetDict;
        private DependencyGraph dg = new DependencyGraph();
        private bool hasChanged;
        private bool nonEmptyConstructor = false;

        public Spreadsheet():
            this(s => true, s => s, "default")
        {
            spreadSheetDict = new Dictionary<string, Cell>();
        }
        
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version):
            base(isValid, normalize, version)
        {
            nonEmptyConstructor = true;
            spreadSheetDict = new Dictionary<string, Cell>();
            // Dictionary values will be written from the xml file
            CopyXML(filePath);
        }


        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version): 
            base(isValid, normalize, version)
        {
            spreadSheetDict = new Dictionary<string, Cell>();
            nonEmptyConstructor = true;
        }

        public override bool Changed { get => hasChanged; protected set => throw new NotImplementedException(); }

        public override object GetCellContents(string name)
        {
            string normName = Normalize(name);
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(normName, varPattern) || name is null)
            {
                throw new InvalidNameException();
            }
            if (!spreadSheetDict.ContainsKey(normName))
            {
                return "";
            }
            return spreadSheetDict[normName].getContentString();
        }

        public override object GetCellValue(string name)
        {
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(name, varPattern) || name is null)
            {
                throw new InvalidNameException();
            }

            if (!spreadSheetDict.ContainsKey(name))
            {
                return "";
            }

            object cellVal = spreadSheetDict[name].CellContents();
            if (cellVal is Formula)
            {
                try
                {
                    return solveCellFormula((Formula)spreadSheetDict[name].CellContents());
                }
                catch
                {
                    return new FormulaError("Cannot evaluate formula.");
                }
            }
            if (cellVal is double)
            {
                return spreadSheetDict[name].CellContents();
            }
            else
            {
                return spreadSheetDict[name].CellContents();
            }
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return spreadSheetDict.Keys;
        }

        public override string GetSavedVersion(string filename)
        {
            try
            {
                string versionInfo = "";
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    versionInfo = reader["version"];
                                    break;
                            }
                        }

                    }
                }
                return versionInfo;
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("There was a problem opening, writing, or closing the file due to: " + e.Message);
            }
        }

        public override void Save(string filename)
        {
            try
            {
                //some non-default settings for our XML writer.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    // write the cells themselves
                    foreach (KeyValuePair<string, Cell> kvp in spreadSheetDict)
                    {
                        string name = kvp.Key;
                        Cell cell = kvp.Value;
                        cell.WriteXML(name, writer);
                    }

                    writer.WriteEndElement(); // Ends the Spreadsheet block
                    writer.WriteEndDocument();
                }
                hasChanged = false;
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("There was a problem opening, writing, or closing the file due to: " + e.Message);
            }
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {

            // If setting new contents then spreadsheet has changed.
            hasChanged = true;
            Console.WriteLine("Set content: " + hasChanged);

            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(name, varPattern) || name is null || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            if (content is null)
            {
                throw new ArgumentNullException();
            }

            double outVal;
            if (content.Length > 0)
            {
                // Content is a double
                if (Double.TryParse(content, out outVal))
                {
                    return SetCellContents(name, outVal);
                }
                // Content is a formula
                string checkFirstIndex = content.Substring(0, 1);
                if (content.Substring(0, 1) == "=")
                {
                    string formulaString = content.Substring(1, content.Length - 1);
                    try
                    {
                        Formula inFormula;
                        if (nonEmptyConstructor == true)
                        {
                            inFormula = new Formula(formulaString, Normalize, IsValid);
                        }
                        else
                        {
                            inFormula = new Formula(formulaString);
                        }
                        return SetCellContents(name, inFormula);
                    }
                    catch (CircularException circ)
                    {
                        throw circ;
                    }
                    catch (FormulaFormatException f)
                    {
                        throw f;
                    }
                }
                // Content is a string
                else
                {
                    return SetCellContents(name, content);
                }
            }
            // Content is an empty string
            else
            {
                return SetCellContents(name, content);
            }
        }


        // Throw errors if name is not valid syntax for a cell name, otherwise have empty string
        protected override IList<string> SetCellContents(string name, double number)
        {

            if (spreadSheetDict.ContainsKey(name))
            {
                spreadSheetDict[name] = new Cell(number);
            }
            else
            {
                spreadSheetDict.Add(name, new Cell(number));
            }

            // If cells are defined later on
            try
            {
                recalcCells(name);
            }
            catch (CircularException)
            {
                throw new CircularException();
            }

            IEnumerator<string> e = dg.GetDependees(name).GetEnumerator();
            List<string> returnList = new List<string>();
            returnList.Add(name);
            while (e.MoveNext())
            {
                returnList.Add(e.Current);
            }
            return returnList;
        }

        protected override IList<string> SetCellContents(string name, string text)
        {
            if (spreadSheetDict.ContainsKey(name))
            {
                Cell prevVal = spreadSheetDict[name];
                if (prevVal.CellContents() is Formula)
                {
                    Formula prevFormula = (Formula) prevVal.CellContents();
                    List<string> eqVariables = (List<string>)prevFormula.GetVariables();
                    foreach (string s in eqVariables)
                    {
                        dg.RemoveDependency(name, s);
                    }
                }
                spreadSheetDict[name] = new Cell(text);
            }
            else
            {
                spreadSheetDict.Add(name, new Cell(text));
            }

            // If value inside, use dg.replaceDependees
            // If string empty, remove dependees and dependents
            IEnumerator<string> e = dg.GetDependees(name).GetEnumerator();
            List<string> returnList = new List<string>();
            returnList.Add(name);
            while (e.MoveNext())
            {
                returnList.Add(e.Current);
            }
            return returnList;
        }

        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            IEnumerable<string> variablesInFormula = formula.GetVariables();
            foreach (string variable in variablesInFormula)
                dg.AddDependency(variable, name);

            // Creating a new cell with content as formula
            Cell newCell = new Cell(formula);

            // Replacing non-empty cell and recalculating dependents.
            if (spreadSheetDict.ContainsKey(name))
            {
                Cell oldCell = spreadSheetDict[name];

                try
                {
                    spreadSheetDict.Remove(name);
                    spreadSheetDict.Add(name, newCell);
                    GetCellsToRecalculate(name);
                    spreadSheetDict[name].SetCellValue(solveCellFormula((Formula)newCell.CellContents()));
                }
                catch 
                {
                    foreach (string variable in variablesInFormula)
                        dg.RemoveDependency(variable, name);
                    spreadSheetDict[name] = oldCell;
                    throw new CircularException();
                }
            }
            if (!spreadSheetDict.ContainsKey(name))
            {
                try
                {
                    spreadSheetDict.Add(name, newCell);
                    GetCellsToRecalculate(name);
                    spreadSheetDict[name].SetCellValue(solveCellFormula((Formula)newCell.CellContents()));
                }
                catch
                {
                    foreach (string variable in variablesInFormula)
                        dg.RemoveDependency(variable, name);
                    throw new CircularException();
                }
            }

            try
            {
                recalcCells(name);
            }
            catch (CircularException)
            {
                throw new CircularException();
            }

            IList<string> dependentList = new List<string> { name };
            foreach (string s in GetDirectDependents(name))
                dependentList.Add(s);
            return dependentList;
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dg.GetDependents(name);
        }


        /// <summary>
        /// Helper method for copying an xml file which already exists.
        /// </summary>
        /// <param name="pathToFile">Takes in file path for the wanted xml file.</param>
        private void CopyXML(string pathToFile)
        {
            try
            {
                string cellname = "";
                {
                    // Create an XmlReader inside this block, and automatically Dispose() it at the end.
                    using (XmlReader reader = XmlReader.Create(pathToFile))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name)
                                {
                                    case "spreadsheet":
                                        if (!GetSavedVersion(pathToFile).Equals(Version))
                                            throw new SpreadsheetReadWriteException("Versions are not equal.");
                                        break;

                                    case "cell":
                                        break;

                                    case "name":
                                        reader.Read();
                                        cellname = reader.Value;
                                        break;

                                    case "contents":
                                        reader.Read();
                                        SetContentsOfCell(cellname, reader.Value);
                                        break;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("There was a problem opening, writing, or closing the file due to: " + e.Message);
            }
        }

        /// <summary>
        /// Lookup function delegate used to evaluate equations with variables.
        /// </summary>
        /// <param name="var">Takes in cell name to find the value of that cell.</param>
        /// <returns></returns>
        private double LookUp(string var)
        {
            return (double) spreadSheetDict[var].GetCellValue();
        }
        /// <summary>
        /// Helper method for solving a cell's formula
        /// </summary>
        /// <param name="inFormula"> Takes in a formula to be solved.</param>
        /// <returns></returns>
        private object solveCellFormula(Formula inFormula)
        {
            try
            {
                return inFormula.Evaluate(LookUp);
            }
            catch
            {
                return new FormulaError("Variable in equation results in an error.");
            }
        }
        /// <summary>
        /// Helper method for recalculating cells if the value of a dependent cell has been
        /// changed.
        /// </summary>
        /// <param name="var">Takes in the cell name which has been changed</param>
        private void recalcCells(string var)
        {
            foreach (string s in GetCellsToRecalculate(var))
            {
                if (spreadSheetDict[s].CellContents() is Formula)
                {
                    spreadSheetDict[s].SetCellValue(solveCellFormula((Formula) spreadSheetDict[s].CellContents()));
                }
            }
        }

        /// <summary>
        /// Cell class which contains all values a class can have
        /// including the cell contents, and cell value.
        /// </summary>
        private class Cell
        {
            object contents;
            object cellValue;
            private delegate double Lookup(string v);

            public Cell(object c)
            {
                contents = c;
                cellValue = contents;
            }
            public object CellContents()
            {
                return contents;
            }

            public object GetCellValue()
            {
                return cellValue;
            }
            // For resetting cell value after solving formula.
            public void SetCellValue(object inValue)
            {
                cellValue = inValue;
            }

            public string getContentString()
            {
                if (contents is Formula)
                {
                    return "=" + contents.ToString();
                }
                else
                {    
                    return contents.ToString();
                }
            }


            public void WriteXML(string name, XmlWriter writer)
            {
                writer.WriteStartElement("cell");
                writer.WriteElementString("name", name);
                writer.WriteElementString("contents", getContentString());
                writer.WriteEndElement();
            }
        }
    }
}
