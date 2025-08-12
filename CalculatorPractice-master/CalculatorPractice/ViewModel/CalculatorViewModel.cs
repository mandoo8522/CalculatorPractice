using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculatorPractice
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CalculatorModel _model;
        private string _display = "0";
        private string _topDisplay = "0";
        private double _firstNumber;
        private string _operation;
        private bool _isNewInput;
        
        

        public string Display
        {
            get => _display;
            set
            {
                _display = value ?? "0";
                OnPropertyChanged();
            }
        }

        public string CalcExp
        {
            get => _topDisplay;
            set
            {
                _topDisplay = value ?? "0";
                OnPropertyChanged();
            }
        }

        public ICommand EnterNumberCommand { get; }
        public ICommand SetOpertaionCommand { get; }
        public ICommand CalculateCommand {  get; }
        public ICommand ClearCommand { get; }
        public ICommand NewDot { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand CEClearCommand { get; } 

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            _isNewInput = true;
            EnterNumberCommand = new RelayCommand(EnterNumber);
            SetOpertaionCommand = new RelayCommand(SetOperation);
            CalculateCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            NewDot=new RelayCommand(GetNewDot);
            BackSpaceCommand = new RelayCommand(BackSpaceNumber);
            CEClearCommand = new RelayCommand(CEClear);
        }

        
        private void EnterNumber(object parameter)
        {
            double temp;
            string number = parameter.ToString();
            
     
            if (number == "+/-")
            {

                if (double.TryParse(Display, out temp))
                {


                    temp = -temp;
                    Display = temp.ToString();


                }

            }
                if (number == "%")
                {

                    if (double.TryParse(Display, out temp))
                    {


                        Display = (_firstNumber *temp/100).ToString();


                    }

                }
            if (double.TryParse(number, out temp))
            {
                // 이미 0인데 0이 또 들어오는 경우 무시한다.
                if (number == "0" && (Display == "0" ))
                {
                    return;
                }

                if (string.IsNullOrEmpty(Display) || Display == "0" || _isNewInput)
                {
                    Display = number; //Display가 비었거나 0이거나 새로운 입력이면 새로운 숫자로 교체
                    _isNewInput = false;
                }
                else
                {
                    Display += number; //숫자가 이미 입력되어있으면 이어서 계속 입력
                }
            }


        }
          
        
        private void BackSpaceNumber(object parameter)
        {
            if (!string.IsNullOrEmpty(Display) && Display.Length > 1)
            {
                Display = Display.Substring(0, Display.Length - 1);
                
            }
            else
            {
                
                Display = "0";
                _isNewInput = true;
            }
        }

        private void GetNewDot(object parameter)
        {
            if (! Display.Contains("."))
            {
                Display += ".";
            }
            
        }

        private void SetOperation(object parameter)
        {
            if (!_isNewInput) Calculate(null);
            _firstNumber=double.Parse(Display);
            _operation = parameter.ToString();
            _isNewInput = true;
            
     
             CalcExp = Display + "" + parameter;
    
        }
        

        private void Calculate(object parameter)
        {
            if (string.IsNullOrEmpty(_operation)) return;
            double secondNumber=double.Parse(Display);
            double result;
            

            try
            {
                switch(_operation)
                {
                    case "+":result = _model.Add(_firstNumber, secondNumber); break;
                    
                    case "-": result = _model.Substract(_firstNumber, secondNumber); break;
                    case "*": result = _model.Multiply(_firstNumber, secondNumber); break;
                    
                    
                    case "/":
                        if (secondNumber == 0)
                        {
                            MessageBox.Show("0으로 나눌 수 없습니다.");
                        }
                        else
                        {
                            return;
                        }
                            result = _model.Divide(_firstNumber, secondNumber);

                        
                        
                        break;


                    default: return;    
                }

                if (result % 1 == 0)
                {
                    Display = ((int)result).ToString(); //소수점 없는 식이면 소수점 안 나오게
                }
                else
                {
                    Display = result.ToString("0.##"); // 소수점이 나오는 식이면 둘째 자리까지만
                }
                _isNewInput = true;
            }
            catch (Exception ex)
            {
                //Display = $"오류:{ex.Message}";
            }

        }
        private void Clear(object parameter)
        {
            
            Display = "0";
            CalcExp = "0";
            _firstNumber = 0;
            _operation = null;
            _isNewInput = true;
        }

        private void CEClear(object parameter)
        {
            Display = "0";
            
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
        
        
    }
   


    


}
