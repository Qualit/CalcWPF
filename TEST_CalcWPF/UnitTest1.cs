using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcWPF.ViewModel;
using System.Collections;
using System.Globalization;

namespace Calc_WPF_TEST
{
    [TestClass]
    public class UnitTest1
    {
        private string separator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
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

        [TestMethod]
        public void decimalSeparatorTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            string referenceNumberStr = "";
            calculatorViewModel.enterDigitCommand.Execute("1");
            referenceNumberStr += "1";

            calculatorViewModel.enterDigitCommand.Execute("2");
            referenceNumberStr += "2";

            calculatorViewModel.enterDigitCommand.Execute("3");
            referenceNumberStr += "3";

            calculatorViewModel.enterDigitCommand.Execute("4");
            referenceNumberStr += "4";

            calculatorViewModel.enterSeparatorCommand.Execute(separator);
            referenceNumberStr += separator;

            calculatorViewModel.enterDigitCommand.Execute("5");
            referenceNumberStr += "5";

            calculatorViewModel.enterEqualsCommand.Execute("=");

            Assert.AreEqual(calculatorViewModel.Display, referenceNumberStr);

            return;
        }

        [TestMethod]
        public void divisionByZeroTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            for(int i=1; i<=10; i++)
            {
                try
                {
                    calculatorViewModel.enterDigitCommand.Execute(Convert.ToString(i));
                    calculatorViewModel.enterBinaryOperationCommand.Execute("/");
                    calculatorViewModel.enterDigitCommand.Execute("0");
                    calculatorViewModel.enterEqualsCommand.Execute("=");
                    Assert.AreEqual(calculatorViewModel.Display, "ERR");
                    calculatorViewModel.enterClearCommand.Execute("C");
                    
                }
                catch (Exception)
                {
                    
                }
                

            }
            return;
        }

        [TestMethod]
        public void sqRootTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            for(int i=4; i<=16384; i=i*4 )
            {
                
                string operand1 = Convert.ToString(i);
                for (int s = 0; s < operand1.Length; s++)
                {
                    string temp = operand1.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }
                calculatorViewModel.enterUnaryOperation.Execute("sqrt");
                double result = Math.Sqrt(i);
                string referenceResult = Convert.ToString(result);
                Assert.AreEqual(calculatorViewModel.Display, referenceResult);
                calculatorViewModel.enterClearCommand.Execute("C");
            }
            return;
        }

        [TestMethod]
        public void negativeSqRootTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            for (int i = 4; i <= 16384; i = i * 4)
            {
                try
                {
                    string operand1 = Convert.ToString(i * -1);
                    for (int s = 0; s < operand1.Length; s++)
                    {
                        string temp = operand1.Substring(s, 1);
                        calculatorViewModel.enterDigitCommand.Execute(temp);
                    }
                    calculatorViewModel.enterUnaryOperation.Execute("sqrt");
                    Assert.AreEqual(calculatorViewModel.Display, "ERR");
                    calculatorViewModel.enterClearCommand.Execute("C");
                }
                catch(Exception)
                {

                }
                
            }
            return;
        }

        [TestMethod]
        public void percentTestMethod()
        {
            calculatorViewModel = new CalculatorViewModel();
            int number = 100;
            for(int i=5; i<=100; i=i+5)
            {
                string numberStr = Convert.ToString(number);
                for (int s = 0; s < numberStr.Length; s++)
                {
                    string temp = numberStr.Substring(s, 1);
                    calculatorViewModel.enterDigitCommand.Execute(temp);
                }
                calculatorViewModel.enterBinaryOperationCommand.Execute("+");
                string percent = Convert.ToString(i);
                for (int p = 0; p < percent.Length; p++)
                {
                    string tmp = percent.Substring(p, 1);
                    calculatorViewModel.enterDigitCommand.Execute(tmp);
                }
                calculatorViewModel.enterUnaryOperation.Execute("%");

                Assert.AreEqual(calculatorViewModel.Display, Convert.ToString(number + i));
                calculatorViewModel.enterClearCommand.Execute("C");
            }
            return;

        }
    
    }
}

