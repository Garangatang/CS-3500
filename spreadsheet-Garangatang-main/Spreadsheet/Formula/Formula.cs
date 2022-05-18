// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
// Modified by Grange Simpson 9/25/21


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /* public delegate string Normalizer(string n);
         public delegate string Validater(bool v);
         public delegate int Lookup(string v);
 */
        private List<string> sepFormula;
        private List<string> variables = new List<string>();

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            sepFormula = GetTokens(formula).ToList();
            makingDoublesUniform(sepFormula);
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            sepFormula = GetTokens(formula).ToList();
            makingDoublesUniform(sepFormula);
            // Since sepFormula is readonly, converted to list for checking index after
            // current in for loop
            List<string> sepList = sepFormula.ToList();
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            double firstNumVal = 0.0;
            double lastNumVal = 0.0;
            // Making sure sepFormula is not empty.
            if (!sepFormula.Any())
            {
                throw new FormulaFormatException("Need at least one token in the equation.");
            }

            // Checking to see if equation contains no operators
            
            if (sepFormula.Count >= 2 && (!sepFormula.Contains("+") &&
                !sepFormula.Contains("-") && !sepFormula.Contains("/")
                && !sepFormula.Contains("*")))
            {
                throw new FormulaFormatException("need an operator in the equation.");
            }

            // Checking first value in equation is valid.
            if (!double.TryParse(sepFormula.First(), out firstNumVal) && sepFormula.First() != "("
                && !Regex.IsMatch(sepFormula.First(), varPattern))
            {
                throw new FormulaFormatException("First value has to be a number or left parenthesis.");
            }
            // Checking last value in equation is valid.
            if (!double.TryParse(sepFormula.Last(), out lastNumVal) && sepFormula.Last() != ")"
                && !Regex.IsMatch(sepFormula.Last(), varPattern))
            {
                throw new FormulaFormatException("Last value has to be a number or right parenthesis.");
            }
            // Checking each element in formula is valid.
            int leftParenthesis = 0;
            int rightParenthesis = 0;
            for (int x = 0; x < sepList.Count; x++)
            { 
                double outVal = 0.0;
                if (operatorCheck(sepList[x]))
                {
                    if (sepList[x].Contains("("))
                    {
                        // Checking value immediately after parenthesis is a double or variable,
                        // or left parenthesis.
                        if (x < sepList.Count - 1 && (!double.TryParse(sepList[x + 1], out outVal) &&
                            !Regex.IsMatch(sepList[x + 1], varPattern) &&
                            !sepList[x + 1].Contains("(")))
                        {
                            throw new FormulaFormatException("Left Parenthesis must be followed by an integer or variable");
                        }
                        leftParenthesis++;
                        continue;
                    }

                    if (sepList[x].Contains(")"))
                    {
                        if (x < sepList.Count - 1 && (sepList[x + 1].Contains("(") ||
                            double.TryParse(sepList[x + 1], out outVal)))
                        {
                            throw new FormulaFormatException("Invalid value following closing parenthesis.");
                        }
                        rightParenthesis++;
                        continue;
                    }
                    // Checking value after +, -, *, / is a double value
                    else
                    {
                        if (x < sepList.Count - 1 && !double.TryParse(sepList[x + 1], out outVal)
                            && !Regex.IsMatch(sepList[x + 1], varPattern) && !sepList[x + 1].Contains("("))
                        {
                            throw new FormulaFormatException("Need a value after +, -, *, /");
                        }
                    }
                }

                // Checking values after variables.
                if (Regex.IsMatch(sepList[x], varPattern))
                {
                    if (x < sepList.Count - 1 && operatorCheck(sepList[x + 1]) && sepList[x + 1] != ")")
                    {
                        string normVal = normalize(sepList[x]);
                        if (!isValid(normVal))
                        {
                            throw new FormulaFormatException("Given variable is not valid for the given equation");
                        }
                        else
                        {
                            sepList[x] = normVal;
                        }
                        continue;
                    }
                    else
                    {
                        string normVal = normalize(sepList[x]);
                        if (!isValid(normVal))
                        {
                            throw new FormulaFormatException("Invalid character in equation after variable.");
                        }
                        else
                        {
                            sepList[x] = normVal;
                        }
                    }
                    
                }
            }

            if (rightParenthesis != leftParenthesis)
            {
                throw new FormulaFormatException("Must have same number of opening and closing parenthesis.");
            }
            sepFormula = sepList;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
       public object Evaluate(Func<string, double> lookup)
        {
            return EquationEval(sepFormula, lookup);
        }

        private object EquationEval(IEnumerable<string> expArr, Func<string, double> variableEvaluator)
        {
            Stack<string> operators = new Stack<string>();
            Stack<double> values = new Stack<double>();
            // While in loop:
            // Check if stack is empty to begin with
            // If empty, check whether int, variable, or operator, add to proper stack
            // If not empty, follow algorithm in assignment
            // If variable, use Lookup
            for (int i = 0; i < sepFormula.Count(); i ++)
            {
                string substring = sepFormula[i];
                if (substring == "" || substring == " ")
                {
                    continue;
                }
                substring.Trim();
                double numberVal;
                // If substring is an integer
                if (double.TryParse(substring, out numberVal))
                {
                    sepFormula[i] = numberVal.ToString();
                    if (values.Count == 0)
                    {
                        values.Push(numberVal);
                        continue;
                    }
                    string operatorVal = (string)operators.Peek();

                    // Checking if / or * is at the top of the 
                    if (IsOnTop(operators, "/") || IsOnTop(operators, "*"))
                    {
                        double newValue;
                        if (operatorVal.Contains("/"))
                        {
                            // Checking for division by zero errors.
                            if (numberVal == 0)
                            {
                                return new FormulaError("Can't divide by zero.");
                            }
                            newValue = values.Pop() / numberVal;
                        }
                        else
                        {
                            newValue = values.Pop() * numberVal;
                        }
                        operators.Pop();
                        values.Push(newValue);
                        continue;
                    }
                    values.Push(numberVal);
                    continue;
                }

                // Checking if substring is not an integer
                if (operatorCheck(substring))
                {
                    // If operators stack is empty.
                    if (operators.Count == 0)
                    {
                        operators.Push(substring);
                        continue;
                    }
                    // Checking for addition or subtraction
                    if ((substring.Contains("+") || substring.Contains("-")))
                    {
                        if (values.Count >= 2 && (IsOnTop(operators, "+") || IsOnTop(operators, "-")))
                        {
                            double value1 = values.Pop();
                            double value2 = values.Pop();
                            double newValue;

                            if (IsOnTop(operators, "+"))
                            {
                                newValue = value2 + value1;
                            }
                            else
                            {
                                newValue = value2 - value1;
                            }

                            operators.Pop();
                            values.Push(newValue);
                            operators.Push(substring);
                            continue;
                        }
                        else
                        {
                            operators.Push(substring);
                            continue;
                        }
                    }
                    // Checking for division, multiplication, or left parenthesis
                    if (substring.Contains("/") || substring.Contains("*") || substring.Contains("("))
                    {
                        operators.Push(substring);
                        continue;
                    }
                    // Checking for right parenthesis
                    if (substring.Contains(")"))
                    {
                        if (values.Count >= 2)
                        {
                            // Checking for addition or subtraction.
                            if (IsOnTop(operators, "+") || IsOnTop(operators, "-"))
                            {
                                double value1 = (double)values.Pop();
                                double value2 = (double)values.Pop();
                                double newValue;

                                if (operators.Peek() == "+")
                                {
                                    newValue = value2 + value1;
                                }
                                else
                                {
                                    newValue = value2 - value1;
                                }
                                values.Push(newValue);
                                operators.Pop();
                            }
                            // Checking for multiplication or division
                            else if (IsOnTop(operators, "/") || IsOnTop(operators, "*"))
                            {
                                double value1 = (double)values.Pop();
                                double value2 = (double)values.Pop();
                                double newValue;

                                if (operators.Peek() == "/")
                                {
                                    if (value1 == 0)
                                    {
                                        return new FormulaError("Can't divide by zero.");
                                    }
                                    else
                                    {
                                        newValue = value2 / value1;
                                    }
                                }
                                else
                                {
                                    newValue = value2 * value1;
                                }

                                values.Push(newValue);
                                operators.Pop();
                            }
                            // Making sure equation is closed.
                            if (operators.Count == 0)
                            {
                                throw new ArgumentException("Need closing parenthesis.");
                            }
                            // Making sure there is a closing parenthesis
                            if (operators.Peek() == "(")
                            {
                                operators.Pop();
                            }
                            // If there is not another value after closing parenthesis
                            if (operators.Count == 0)
                            {
                                continue;
                            }
                            // Final check for multiplication or division.
                            if (IsOnTop(operators, "/") || IsOnTop(operators, "*"))
                            {
                                double value1 = (double)values.Pop();
                                double value2 = (double)values.Pop();
                                double newValue;

                                if (operators.Peek() == "/")
                                {
                                    if (value1 == 0)
                                    {
                                        return new FormulaError("Can't divide by zero.");
                                    }
                                    else
                                    {
                                        newValue = value2 / value1;
                                    }
                                }
                                else
                                {
                                    newValue = value2 * value1;
                                }

                                values.Push(newValue);
                                operators.Pop();
                            }
                            continue;
                        }
                    }
                }
                // Checking for variables
                else
                {
                    // Adding variables to list so can be returned by getVariables.
                    variables.Add(substring);

                    if (values.Count == 0)
                    {
                        values.Push(variableEvaluator(substring));
                        continue;
                    }
                    if (IsOnTop(operators, "/") || IsOnTop(operators, "*"))
                    {
                        double newValue;
                        double variableVal = variableEvaluator(substring);
                        if (IsOnTop(operators, "/"))
                        {
                            if (variableVal == 0)
                            {
                                return new FormulaError("Can't divide by zero.");
                            }
                            else
                            {
                                newValue = values.Pop() / variableVal;
                            }
                        }

                        else
                        {
                            newValue = values.Pop() * variableVal;
                        }
                        operators.Pop();
                        values.Push(newValue);
                        continue;

                    }
                    values.Push(variableEvaluator(substring));
                    continue;
                }
            }

            // Operator stack is not empty yet, and 2 values are left
            if (operators.Count != 0 && values.Count >= 2)
            {

                double value1 = values.Pop();
                double value2 = values.Pop();
                double newVal;

                if (IsOnTop(operators, "+"))
                {
                    newVal = value2 + value1;
                    operators.Pop();
                    values.Push(newVal);
                }
                else
                {
                    newVal = value2 - value1;
                    operators.Pop();
                    values.Push(newVal);
                }
            }
            return (double)values.Pop();
        }



        private bool operatorCheck(string s)
        {
            // Ensure not accidentally catching scientific notation.
            if (Double.TryParse(s, out double checkVal))
                return false;
            if (s.Contains("/") || s.Contains("*"))
                return true;
            if (s.Contains("+") || s.Contains("-"))
                return true;
            if (s.Contains("(") || s.Contains(")"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            List<string> returnVariables = new List<string>();
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            foreach (string s in sepFormula)
            {
                if (Regex.IsMatch(s, varPattern))
                {
                    if (returnVariables.Contains(s))
                    {
                        continue;
                    }
                    else
                    {
                        returnVariables.Add(s);
                    }
                }
                else
                {
                    continue;
                }
                
            }
            variables = returnVariables;
            return returnVariables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string finalString = "";
            foreach (string x in sepFormula)
            {
                finalString += x;
            }
            return finalString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null) || !(obj is Formula))
            {
                return false;
            }
            else
            {
                //int comparison = (obj.ToString()).CompareTo(this.ToString());
                string firstComp = obj.ToString();
                string secondComp = this.ToString();
                if (firstComp.Equals(secondComp))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1 is null || f2 is null)
            {
                return false;
            }
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            string returnVal = this.ToString();
            return returnVal.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Method to check whether a given operator is on top of the operators stack.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns>bool</returns>
        private static bool IsOnTop(Stack<string> s, string c)
        {
            return s.Peek() == c && s.Count > 0;
        }

        private void makingDoublesUniform(List<string> inList)
        {
            for (int i = 0; i < inList.Count(); i ++)
            {
                if (Double.TryParse(inList[i], out double outVal))
                {
                    inList[i] = outVal.ToString();
                }
            }
            sepFormula = inList;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {

        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }



    static class StackExtensions
    {
        
        
    }
}