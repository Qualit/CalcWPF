using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CalcWPF.Model
{
    class CalculatorModel
    {
        private static readonly log4net.ILog log =
                       log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private double result;
        private double accumulator;
        private string lastOperation;

        public enum CalculatorState { START, ACCUMULATE, COMPUTE, EVALUATED, POINT, ERROR };
        private CalculatorState calculatorState;

        public string Separator
        {
            get
            {
                return NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            }
        }
        public string Display { get; set; }

        public CalculatorState CurrentCalculatorState
        {
            get
            {
                return calculatorState;
            }
            set
            {
                this.calculatorState = value;
            }

        }


        public CalculatorModel()
        {
            Display = "0";
            calculatorState = CalculatorState.START;
        }
        public void enterBinaryOperation(string operation)
        {

            double currentValue = double.Parse(Display);
            System.Diagnostics.Debug.WriteLine("currentValue:" + currentValue);
            switch (CurrentCalculatorState)
            {
                // calculator was accumulating digits and now operation arrived
                // so we execute previous operation and store this operation execution
                case CalculatorState.ACCUMULATE:
                    {
                        switch (lastOperation)
                        {
                            case "+":
                                {
                                    log.Info("ADDITION, operand1: " + accumulator + " operand2: " + currentValue);
                                    accumulator += currentValue;
                                    log.Info("ADDITION, result: " + accumulator);
                                    break;
                                }

                            case "-":
                                {
                                    log.Info("SUBTRACTION, operand1: " + accumulator + " operand2: " + currentValue);
                                    accumulator -= currentValue;
                                    log.Info("SUBTRACTION, result: " + accumulator);
                                    break;
                                }
                            case "*":
                                {
                                    log.Info("MULTIPLICATION, operand1: " + accumulator + " operand2: " + currentValue);
                                    accumulator *= currentValue;
                                    log.Info("MULTIPLICATION, result: " + accumulator);
                                    break;
                                }
                            case "/":
                                {
                                    log.Info("DIVISION, operand1: " + accumulator + " operand2: " + currentValue);
                                    if (currentValue == 0)
                                    {
                                        throw new System.DivideByZeroException();
                                    }
                                    accumulator /= currentValue;
                                    log.Info("DIVISION, result: " + accumulator);
                                    break;
                                }

                            default:
                                {
                                    accumulator = currentValue;
                                    break;
                                }

                        }
                        lastOperation = operation;
                        Display = accumulator.ToString();
                        CurrentCalculatorState = CalculatorState.COMPUTE;
                        return;
                    }
                case CalculatorState.EVALUATED:
                    {
                        CurrentCalculatorState = CalculatorState.COMPUTE;
                        lastOperation = operation;
                        return;
                    }
                default: return;
            }

        }

        public void enterDigit(string digit)
        {
            if (digit.Length > 1)
            {
                throw new ArgumentException();
                //digit = digit.Substring(0, 1);
            }
            if (!(Regex.IsMatch(digit, "[0-9]")))
            {
                throw new ArgumentException();
            }
            switch (CurrentCalculatorState)
            {
                case CalculatorState.START:
                    {
                        Display = digit;
                        CurrentCalculatorState = CalculatorState.ACCUMULATE;
                        return;
                    }

                case CalculatorState.ACCUMULATE:
                    {
                        Display = Display + digit;
                        return;
                    }

                case CalculatorState.POINT:
                    {
                        Display = Display + digit;
                        return;
                    }

                case CalculatorState.COMPUTE:
                    {
                        CurrentCalculatorState = CalculatorState.ACCUMULATE;
                        Display = digit;
                        return;
                    }
                case CalculatorState.EVALUATED:
                    {
                        accumulator = 0;
                        lastOperation = "NO_OP";
                        Display = digit;
                        CurrentCalculatorState = CalculatorState.ACCUMULATE;
                        return;
                    }

            }

        }

        public void enterEquals()
        {
            //log.Debug("Equals!!!!!!");
            double currentValue = double.Parse(Display);
            System.Diagnostics.Debug.WriteLine("currentValue:" + currentValue);
            if (CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                switch (lastOperation)
                {
                    case "+":
                        {
                            log.Info("ADDITION, operand1: " + accumulator + " operand2: " + currentValue);
                            accumulator += currentValue;
                            log.Info("ADDITION, result: " + accumulator);
                            break;
                        }

                    case "-":
                        {
                            log.Info("SUBTRACTION, operand1: " + accumulator + " operand2: " + currentValue);
                            accumulator -= currentValue;
                            log.Info("SUBTRACTION, result: " + accumulator);
                            break;
                        }
                    case "*":
                        {
                            log.Info("MULTIPLICATION, operand1: " + accumulator + " operand2: " + currentValue);
                            accumulator *= currentValue;
                            log.Info("MULTIPLICATION, result: " + accumulator);
                            break;
                        }
                    case "/":
                        {
                            log.Info("DIVISION, operand1: " + accumulator + " operand2: " + currentValue);
                            if (currentValue == 0)
                            {
                                throw new System.DivideByZeroException();
                            }
                            accumulator /= currentValue;
                            log.Info("DIVISION, result: " + accumulator);
                            break;
                        }

                    default:
                        {
                            accumulator = currentValue;
                            break;
                        }

                }
                Display = accumulator.ToString();
                lastOperation = "NO_OP";
                CurrentCalculatorState = CalculatorState.EVALUATED;

            }
            else if (CurrentCalculatorState == CalculatorState.COMPUTE)
            {
                lastOperation = "NO_OP";
                CurrentCalculatorState = CalculatorState.EVALUATED;
            }

        }

        public void enterSeparator()
        {
            //System.Diagnostics.Debug.WriteLine("current state:" + CurrentCalculatorState);
            if (CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                if (!Display.Contains(Separator))
                {
                    Display += Separator;
                }
            }

        }

        public void enterUnaryOperation(string operation)
        {
            if (CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                switch (operation)
                {
                    case "+/-":
                        {
                            double tmp = Double.Parse(Display);
                            log.Info("CHANGE_SIGN, operand: " + tmp);
                            Display = Convert.ToString(tmp * (-1));
                            log.Info("CHANGE_SIGN, result: " + Display);
                            break;
                        }
                    case "%":
                        {
                            double currentDisplay = Double.Parse(Display);
                            double percent = currentDisplay / 100;
                            switch (lastOperation)
                            {
                                case "+":
                                    {
                                        accumulator += accumulator * percent;
                                        break;
                                    }

                                case "-":
                                    {
                                        accumulator -= accumulator * percent;
                                        break;
                                    }
                                case "*":
                                    {
                                        accumulator *= accumulator * percent;
                                        break;
                                    }
                                case "/":
                                    {
                                        if (percent == 0)
                                        {
                                            throw new System.DivideByZeroException();
                                        }
                                        accumulator /= accumulator * percent;
                                        break;
                                    }

                                default:
                                    {

                                        break;
                                    }

                            }
                            Display = accumulator.ToString();
                            lastOperation = "NO_OP";
                            CurrentCalculatorState = CalculatorState.EVALUATED;
                            break;
                        }
                    case "sqrt":
                        {
                            double tmp = Double.Parse(Display);
                            log.Info("SQ_ROOT, operand: " + tmp);
                            if (tmp < 0)
                            {
                                throw new System.ArithmeticException();
                            }
                            Display = Convert.ToString(Math.Sqrt(tmp));
                            log.Info("SQ_ROOT, result: " + Display);
                            break;
                        }
                }
            }
            else if (CurrentCalculatorState == CalculatorState.EVALUATED)
            {
                switch (operation)
                {
                    case "+/-":
                        {
                            double tmp = Double.Parse(Display);
                            log.Info("CHANGE_SIGN, operand: " + tmp);
                            Display = Convert.ToString(tmp * (-1));
                            log.Info("CHANGE_SIGN, result: " + Display);
                            accumulator = Double.Parse(Display);
                            break;
                        }
                    case "%":
                        {
                            break;
                        }
                    case "sqrt":
                        {
                            double tmp = Double.Parse(Display);
                            log.Info("SQ_ROOT, operand: " + tmp);
                            if (tmp < 0)
                            {
                                throw new System.ArithmeticException();
                            }
                            Display = Convert.ToString(Math.Sqrt(tmp));
                            log.Info("SQ_ROOT, result: " + Display);
                            accumulator = Double.Parse(Display);
                            break;
                        }
                }
            }

        }
        public void enterClear()
        {
            log.Info("CLEAR");
            Display = "0";
            accumulator = 0;
            lastOperation = "NO_OP";
            CurrentCalculatorState = CalculatorState.START;
        }
    }
}
