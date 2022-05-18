using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{

    public static class Evaluator
    {

        public delegate int Lookup(string v);

        private static Stack<string> operators = new Stack<string>();
        private static Stack<double> values = new Stack<double>();
        /// <summary>
        /// Driver method for EquationEval
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns>final in answer</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            operators.Clear();
            values.Clear();
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            substrings = substrings.Where(e => e != "").ToArray();
            substrings = substrings.Where(e => e != " ").ToArray();
            // Check if substrings is empty, if it is the exp was not right and throw error

            if (substrings.Length == 0)
            {
                throw new ArgumentException("Must enter an equation.");
            }
            // Just return value if singular number/value given.
            if (substrings.Length == 1)
            {
                int numberVal;
                if (int.TryParse(substrings[0], out numberVal)) 
                {
                    return numberVal;
                }
                if (variableCheck(substrings[0]))
                {
                    return variableEvaluator(substrings[0]);
                }
                else
                {
                    throw new ArgumentException("Not an equation.");
                }
            }

            return EquationEval(substrings, variableEvaluator);
        }
        /// <summary>
        /// Equation evaluation function
        /// </summary>
        /// <param name="expArr"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns>Answer for equation</returns>
        private static int EquationEval(String[] expArr, Lookup variableEvaluator)
        {
            // While in loop:
            // Check if stack is empty to begin with

            // If empty, check whether int, variable, or operator, add to proper stack

            // If not empty, follow algorithm in assignment

            // If variable, use Lookup
            foreach (string substring in expArr)
            {
                if (substring == "" || substring == " ")
                {
                    continue;
                }
                substring.Trim();
                int numberVal;
                // If substring is an integer
                if (int.TryParse(substring, out numberVal))
                {
                    if (values.Count == 0)
                    {
                        values.Push(numberVal);
                        continue;
                    }
                    string operatorVal = (string)operators.Peek();

                    // Checking if / or * is at the top of the 
                    if (operators.IsOnTop("/") || operators.IsOnTop("*"))
                    {
                        double newValue;

                        if (operatorVal.Contains("/"))
                        {
                            // Checking for division by zero errors.
                            if (numberVal == 0)
                            {
                                throw new ArgumentException("Can't divide by zero.");
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
                if (!int.TryParse(substring, out numberVal))
                {
                    //string operatorValue = operators.Peek()

                    // Checking for variables
                    if (variableCheck(substring))
                    {
                        if (values.Count == 0)
                        {
                            values.Push(variableEvaluator(substring));
                            continue;
                        }
                        if (operators.IsOnTop("/") || operators.IsOnTop("*"))
                        {
                            double newValue;
                            double variableVal;
                            // Catching any variables which do not belong within the equation.
                            try
                            {
                                variableVal = variableEvaluator(substring);
                            }
                            catch
                            {
                                throw new NullReferenceException();
                            }


                            if (operators.IsOnTop("/"))
                            {
                                if (variableVal == 0)
                                {
                                    throw new DivideByZeroException("Cannot divide by zero.");
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

                    // If operators stack is empty.
                    if (operators.Count == 0)
                    {
                        operators.Push(substring);
                        continue;
                    }

                    // Checking for addition or subtraction
                    if ((substring.Contains("+") || substring.Contains("-")))
                    {
                        if (values.Count >= 2 && (operators.IsOnTop("+") || operators.IsOnTop("-")))
                        {
                            double value1 = values.Pop();
                            double value2 = values.Pop();
                            double newValue;

                            if (operators.IsOnTop("+"))
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
                            if (operators.IsOnTop("+") || operators.IsOnTop("-"))
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
                            else if (operators.IsOnTop("/") || operators.IsOnTop("*"))
                            {
                                double value1 = (double)values.Pop();
                                double value2 = (double)values.Pop();
                                double newValue;

                                if (operators.Peek() == "/")
                                {
                                    if (value1 == 0)
                                    {
                                        throw new DivideByZeroException("Can't divide by zero");
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
                            if (operators.IsOnTop("/") || operators.IsOnTop("*"))
                            {
                                double value1 = (double)values.Pop();
                                double value2 = (double)values.Pop();
                                double newValue;

                                if (operators.Peek() == "/")
                                {
                                    if (value1 == 0)
                                    {
                                        throw new DivideByZeroException("Can't divide by zero");
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

                // Taking care of invalid elements
                else
                {
                    throw new ArgumentException("Invalid character in equation");
                }
            }

            // Operator stack is not empty yet, and 2 values are left
            if (operators.Count != 0 && values.Count >= 2)
            {
                
                double value1 = values.Pop();
                double value2 = values.Pop();
                double newVal;

                if (operators.IsOnTop("+"))
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

            // If too many operators were added, or residual parenthesis left
            if (operators.Count != 0 && values.Count < 2 && operators.Peek() != "(")
            {
                throw new ArgumentException("Too many operators.");
            }
            // If too many values were added
            if (operators.Count == 0 && values.Count >= 2)
            {
                throw new ArgumentException("Too many values for number of operators.");
            }

            return (int)values.Pop();
        }
        /// <summary>
        /// Method to check whether a given operator is on top of the operators stack.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns>bool</returns>
        private static bool IsOnTop(this Stack<string> s, string c)
        {
            return s.Peek() == c && s.Count > 0;
        }
        /// <summary>
        /// Checking if a there is a variable instead of an operator
        /// </summary>
        /// <param name="s"></param>
        /// <returns>bool</returns>
        private static bool variableCheck(string s)
        {
            if (s == "/" || s == "*")
                return false;
            if (s == "+" || s == "-")
                return false;
            if (s == "(" || s == ")")
                return false;
            else
                return true;
        }
    }
}
