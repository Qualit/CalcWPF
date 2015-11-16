using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcWPF.ViewModel;
using System.Collections;

namespace Calc_WPF_TEST
{
    [TestClass]
    public class UnitTest1
    {
        private CalculatorViewModel calculatorViewModel;

        [TestMethod]
        public void initializationTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            Assert.AreEqual("0", calculatorViewModel.Display);
        }

        [TestMethod]
        public void enterDigitsTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            string inputSequence = "";
            for (int i = 0; i < 10; i++)
            {
                string digit = Convert.ToString(i);
                inputSequence += digit;
                calculatorViewModel.enterDigitCommand.Execute(digit);
            }
            System.Diagnostics.Debug.WriteLine("inputSequence:" + inputSequence);
            Assert.AreEqual(inputSequence, calculatorViewModel.Display);

        }

        [TestMethod]
        public void additionTestMethod1()
        {
            calculatorViewModel = new CalculatorViewModel();
            for (int i = 0; i < 10; i++)
            {
                int number1 = i * 5000 + 5;
                int number2 = i * 3000 + 3;
                int number3 = i * 1000 + 1;
                int refernceResultNumber = number1 + number2 + number3;

                string operand1 = Convert.ToString(number1);
                for (int s = 0; s < operand1.Length; s++)
                {
                    string temp = operand1.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }

                calculatorViewModel.enterBinaryOperationCommand.Execute("+");

                string operand2 = Convert.ToString(number2);
                for (int s = 0; s < operand2.Length; s++)
                {
                    string temp = operand2.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }
                calculatorViewModel.enterBinaryOperationCommand.Execute("+");

                string operand3 = Convert.ToString(number3);
                for (int s = 0; s < operand3.Length; s++)
                {
                    string temp = operand3.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }

                calculatorViewModel.enterEqualsCommand.Execute("=");

                Assert.AreEqual(calculatorViewModel.Display, Convert.ToString(refernceResultNumber));
                System.Diagnostics.Debug.WriteLine("Reference result:" + Convert.ToString(refernceResultNumber));
                System.Diagnostics.Debug.WriteLine("Calculator result:" + calculatorViewModel.Display);
            }
            return;
        }

        [TestMethod]
        public void subtractionTestMethod1()
        {
            calculatorViewModel = new CalculatorViewModel();
            for (int i = 0; i < 10; i++)
            {
                int number1 = i * 4000 + 4;
                int number2 = i * 2000 + 2;
                int number3 = i * 1000 + 1;
                int refernceResultNumber = number1 - number2 - number3;

                string operand1 = Convert.ToString(number1);
                for (int s = 0; s < operand1.Length; s++)
                {
                    string temp = operand1.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }

                calculatorViewModel.enterBinaryOperationCommand.Execute("-");

                string operand2 = Convert.ToString(number2);
                for (int s = 0; s < operand2.Length; s++)
                {
                    string temp = operand2.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }
                calculatorViewModel.enterBinaryOperationCommand.Execute("-");

                string operand3 = Convert.ToString(number3);
                for (int s = 0; s < operand3.Length; s++)
                {
                    string temp = operand3.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }

                calculatorViewModel.enterEqualsCommand.Execute("=");

                Assert.AreEqual(calculatorViewModel.Display, Convert.ToString(refernceResultNumber));
                System.Diagnostics.Debug.WriteLine("Reference result:" + Convert.ToString(refernceResultNumber));
                System.Diagnostics.Debug.WriteLine("Calculator result:" + calculatorViewModel.Display);
            }
            return;
        }
    }
}

