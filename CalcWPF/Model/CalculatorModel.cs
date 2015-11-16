using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CalcWPF.Model
{
    class CalculatorModel
    {
        private double result;
        private double accumulator;
        private string lastOperation;
        private bool freshDisplay;
        //private string display;

        public enum CalculatorState {START,ACCUMULATE,COMPUTE,EVALUATED, POINT,ERROR};
        private CalculatorState calculatorState;

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
            freshDisplay = true;
        }
        public void enterBinaryOperation(string operation)
        {
            
            double currentValue = double.Parse(Display);
            System.Diagnostics.Debug.WriteLine("currentValue:" + currentValue);
            switch(CurrentCalculatorState)
            {
                    // calculator was accumulating digits and now operation arrived
                    // so we execute previous operation and store this operation execution
                    case CalculatorState.ACCUMULATE:
                        {
                            switch (lastOperation)
                            {
                                case "+":
                                    {
                                        accumulator += currentValue;
                                        break;
                                    }

                                case "-":
                                    {
                                        accumulator -= currentValue;
                                        break;
                                    }
                                case "*":
                                    {
                                        accumulator *= currentValue;
                                        break;
                                    }
                                case "/":
                                    {
                                        if(currentValue == 0)
                                        {
                                            throw new System.DivideByZeroException();
                                        }
                                        accumulator /= currentValue;
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
            if(digit.Length>1)
            {
                throw new ArgumentException();
                //digit = digit.Substring(0, 1);
            }
            if(!(Regex.IsMatch(digit,"[0-9]")))
            {
                throw new ArgumentException();
            }
            switch(CurrentCalculatorState)
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
            double currentValue = double.Parse(Display);
            System.Diagnostics.Debug.WriteLine("currentValue:" + currentValue);
            if(CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                switch (lastOperation)
                {
                    case "+":
                        {
                            accumulator += currentValue;
                            break;
                        }

                    case "-":
                        {
                            accumulator -= currentValue;
                            break;
                        }
                    case "*":
                        {
                            accumulator *= currentValue;
                            break;
                        }
                    case "/":
                        {
                            if (currentValue == 0)
                            {
                                throw new System.DivideByZeroException();
                            }
                            accumulator /= currentValue;
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
            else if(CurrentCalculatorState == CalculatorState.COMPUTE)
            {
                lastOperation = "NO_OP";
                CurrentCalculatorState = CalculatorState.EVALUATED;
            }

        }

        public void enterSeparator()
        {
            System.Diagnostics.Debug.WriteLine("current state:" + CurrentCalculatorState);
            if(CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                if (!Display.Contains(","))
                {
                    Display += ",";
                }
            }

        }

        public void enterUnaryOperation(string operation)
        {
            if(CurrentCalculatorState == CalculatorState.ACCUMULATE)
            {
                switch (operation)
                {
                    case "+/-":
                        {
                            double tmp = Double.Parse(Display);
                            Display = Convert.ToString(tmp * (-1));
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
                                        accumulator += accumulator*percent;
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
                            if(tmp<0)
                            {
                                throw new System.ArithmeticException();
                            }
                            Display = Convert.ToString(Math.Sqrt(tmp));
                            break;
                        }
                }
            }
            else if(CurrentCalculatorState == CalculatorState.EVALUATED)
            {
                switch (operation)
                {
                    case "+/-":
                        {
                            double tmp = Double.Parse(Display);
                            Display = Convert.ToString(tmp * (-1));
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
                            if (tmp < 0)
                            {
                                throw new System.ArithmeticException();
                            }
                            Display = Convert.ToString(Math.Sqrt(tmp));
                            accumulator = Double.Parse(Display);
                            break;
                        }
                }
            }
            
        }
        public void enterClear()
        {
            Display = "0";
            accumulator = 0;
            lastOperation = "NO_OP";
            CurrentCalculatorState = CalculatorState.START;
        }
    }
}
