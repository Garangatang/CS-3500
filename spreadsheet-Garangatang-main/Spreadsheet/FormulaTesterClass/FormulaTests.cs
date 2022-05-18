using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTesterClass
{
    [TestClass]
    public class FormulaTesterClass
    {
        [TestMethod]
        public void CheckingEquationOutputSimple()
        {
            Formula test1 = new Formula("5-5");
            Assert.AreEqual(0, test1.Evaluate(s => 3));
        }

        [TestMethod]
        public void CheckEquationOutputParenthesis()
        {
            Formula test1 = new Formula("(10 + 4)");
            Assert.AreEqual(14, test1.Evaluate(s => 10));
        }

        [TestMethod]
        public void CheckEquationOutputComplexEq()
        {
            Formula test1 = new Formula("(10 + 4) - 5 / 5");
            Assert.AreEqual(13, test1.Evaluate(s => 10));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckExceptionsUnequalParenthesisLeft() 
        {
            Formula test1 = new Formula("(10 + 4 - 2 / 10");
            test1.Evaluate(s => 10);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckExceptionUnequalParenthesisRight()
        {
            Formula test1 = new Formula("10 + 4 - 2 / 10)");
            test1.Evaluate(s => 10);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckInvalidEqleftParenAfterNum()
        {
            Formula test1 = new Formula("(10 / 2(");
            test1.Evaluate(s => 5);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckLastValInEqValid()
        {
            Formula test1 = new Formula("(10 / 2)+");
            test1.Evaluate(s => 10);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckFirstValInEqationValid()
        {
            Formula test1 = new Formula("+(10 / 2)");
            test1.Evaluate(s => 10);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckOperatorAfterParenthesis()
        {
            Formula test1 = new Formula("((10 / 2)+)");
            test1.Evaluate(s => 20);
        }

        [TestMethod]
        public void CheckVariableReturn()
        {
            Formula test1 = new Formula("(A4 + 2)");
            Assert.AreEqual(14, test1.Evaluate(s => 12));
        } 

        [TestMethod]
        public void CheckSimpleDivisionByZeroFormulaError()
        {
            Formula test1 = new Formula("1 / 0");
            Assert.AreEqual(new FormulaError("Can't divide by zero."), test1.Evaluate(s => 12));
        }
        
        [TestMethod]
        public void CheckVariableDivisionByZeroFormulaError()
        {
            Formula test1 = new Formula("a3 / 0");
            Assert.AreEqual(new FormulaError("Can't divide by zero."), test1.Evaluate(s => 12));
        }
        [TestMethod]
        public void CheckEquationWithNormAndVal()
        {
            Formula test1 = new Formula("b2 / 10", Normalizer2, Validator2);
            Assert.AreEqual(1, test1.Evaluate(VarLookUp));
        }
        [TestMethod]
        public void CheckEquationWithNormAndVal2()
        {
            Formula test1 = new Formula("A4 / 10", Normalizer1, Validator1);
            Assert.AreEqual(1, test1.Evaluate(s => 10));
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckEquationWithNormalAndValException()
        {
            Formula test1 = new Formula("A4 / 10", Normalizer1, Validator2);
            test1.Evaluate(s => 10);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CheckEquationWithNormalAndValException2()
        {
            Formula test1 = new Formula("A4 / 10", Normalizer2, Validator1);
            test1.Evaluate(s => 10);
        }

        [TestMethod]
        public void CheckEquationWithNormalAndValMultipleVariables()
        {
            Formula test1 = new Formula("B2 / 10 + 24 + b3", Normalizer2, Validator2);
            Assert.AreEqual(37, test1.Evaluate(VarLookUp));
        }
        [TestMethod]
        public void CheckToStringNoNormOrVal()
        {
            Assert.AreEqual("x+Y", new Formula("x + Y").ToString());
        }
        [TestMethod]
        public void CheckToStringWithNormAndVal()
        {
            Assert.AreEqual("X+Y", new Formula("x + y", Normalizer2, Validator2).ToString());
        }
        [TestMethod]
        public void TestingEqualsFalse()
        {
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("z2 + x1")));
        }
        [TestMethod]
        public void TestingEqualsTrue()
        {
            Assert.IsTrue(new Formula("x1+y2", Normalizer2, Validator2).Equals(new Formula("X1 + Y2")));
        }

            //Normalizers and Validators
        static bool Validator1(string s)
        {
            return Char.IsLower(s[0]);
        }
        static string Normalizer1(string s)
        {
            return s.ToLower();
        }

        static bool Validator2(string s)
        {
            return Char.IsUpper(s[0]);
        }
        static string Normalizer2(string s)
        {
            return s.ToUpper();
        }

            // Lookups
        public static double VarLookUp(string var)
        {
            var.Trim();
            if (var.Contains("A1"))
                return 2;
            if (var.Contains("A2"))
                return 4;
            if (var.Contains("A3"))
                return 6;
            if (var.Contains("B1"))
                return 8;
            if (var.Contains("B2"))
                return 10;
            if (var.Contains("B3"))
                return 12;

            return 0;
        }
    }

}
