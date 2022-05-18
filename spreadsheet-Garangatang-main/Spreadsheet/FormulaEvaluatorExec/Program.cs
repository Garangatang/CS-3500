using System;
using FormulaEvaluator;
using static FormulaEvaluator.Evaluator;

namespace FormulaEvaluatorExec
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test("(3 + 2) / (5 - 4)", null, 5);
        }

        public static void Test(string formula, Lookup lookup, int expected)
        {
            int actual = Evaluator.Evaluate(formula, lookup);
            if (actual == expected)
                Console.WriteLine($"PASS: {formula} evaluated to {expected}");
            else
                Console.WriteLine($"FAIL: {formula} evaluated to {actual} instead of {expected}");
        }
    }
}
