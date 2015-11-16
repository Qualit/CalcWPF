using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalcWPF.ViewModel
{
    public class CalculatorViewModel : ViewModelBase
    {
        private Model.CalculatorModel calculatorModel;
        private bool errorMode;
        public ICommand enterDigitCommand { get; set; }
        public ICommand enterBinaryOperationCommand { get; set; }
        public ICommand enterEqualsCommand { get; set; }
        public ICommand enterUnaryOperation { get; set; }
        public ICommand enterClearCommand { get; set; }
        public ICommand enterSeparatorCommand { get; set; }


        public string Display
        {
            get
            {
                return calculatorModel.Display;
            }
            set
            {
                if(value == calculatorModel.Display)
                {
                    return;
                }
                calculatorModel.Display = value;
                base.OnPropertyChanged("Display");
            }
        }

        public CalculatorViewModel()
        {
            calculatorModel = new Model.CalculatorModel();
            enterDigitCommand = new RelayCommand(new Action<object>(delegate(object obj)
            {
                calculatorModel.enterDigit(obj as string);
                base.OnPropertyChanged("Display");
            }), o=>buttonsActive());

            enterBinaryOperationCommand = new RelayCommand(new Action<object>(delegate(object obj)
            {
                try
                {
                    calculatorModel.enterBinaryOperation(obj as string);
                    base.OnPropertyChanged("Display");
                }
                catch (Exception)
                {

                    errorMode = true;
                }
            }), o=>buttonsActive());

            enterUnaryOperation = new RelayCommand(new Action<object>(delegate(object obj)
                {
                    try
                    {
                        calculatorModel.enterUnaryOperation(obj as string);
                        base.OnPropertyChanged("Display");
                    }
                    catch (Exception)
                    {

                        errorMode = true;
                    }
                }), o => buttonsActive());

            enterEqualsCommand = new RelayCommand(new Action<object>(delegate(object obj)
            {
                try
                {
                    calculatorModel.enterEquals();
                    base.OnPropertyChanged("Display");
                }
                catch (Exception)
                {

                    errorMode = true;
                }
            }), o => buttonsActive());

            enterClearCommand = new RelayCommand(new Action<object>(delegate(object obj)
                {
                    calculatorModel.enterClear();
                    errorMode = false;
                    base.OnPropertyChanged("Display");
                }));
            enterSeparatorCommand = new RelayCommand(new Action<object>(delegate(object obj)
                {
                    calculatorModel.enterSeparator();
                    base.OnPropertyChanged("Display");
                }), o => buttonsActive());
        }

        public bool buttonsActive()
        {
            return !errorMode;
        }

    }
}
