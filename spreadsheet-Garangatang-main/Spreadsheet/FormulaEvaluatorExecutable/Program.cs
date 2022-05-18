using System;
using FormulaEvaluator;
using static FormulaEvaluator.Evaluator;

namespace FormulaEvaluatorExecutable
{ 
    class Program
    {
        static void Main(string[] args)
        {
            Test("(10*2", null, 20);
            Test("(5*2) / (1*5)+(6+2)", null, 10);
            Test("(3+2)/(5-4)+(3*10)", null, 35);
            Test("3/2", null, 1);
            Test("5/2", null, 2);
            Test("3+2", null, 5);
            Test("10 + 2", null, 12);
            Test("10 * 5", null, 50);
            Test("10 * A4", ZeroLookUp, 0);
            Test("10 * A3", VarLookUp, 60);
            try
            {
                Test("10 / 0", null, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try
            {
                Test("10 / A5", VarLookUp, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try
            {
                Test("10 / %", null, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try
            {
                Test("(10*2", null, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Method to test whether the Evaluate method returns the correct value or not.
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="lookup"></param>
        /// <param name="expected"></param>
        public  static void Test(string formula, Lookup lookup, int expected)
        {
            int actual = Evaluator.Evaluate(formula, lookup);
            if (actual == expected)
                Console.WriteLine($"PASS: {formula} evaluated to {expected}");
            else
                Console.WriteLine($"FAIL: {formula} evaluated to {actual} instead of {expected}");
        }
        /// <summary>
        /// Simple lookup returning 0 for a Lookup
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static int ZeroLookUp(string var)
        {
            return 0;
        }
        /// <summary>
        /// Method for testing out different variable values.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static int VarLookUp(string var)
        {
            var.Trim();
            if (var.Contains("A1"))
                return 2;
            if (var.Contains("A2"))
                return 4;
            if (var.Contains("A3"))
                return 6;
            return 0;
        }
    }

}
