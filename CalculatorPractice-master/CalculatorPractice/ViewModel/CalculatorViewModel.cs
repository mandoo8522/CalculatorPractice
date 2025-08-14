using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //public string CalcExp
        //{
        //    get => _topDisplay;
        //    set
        //    {
        //        _topDisplay = value ?? "0";
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand EnterNumberCommand { get; }
        public ICommand SetOpertaionCommand { get; }
        public ICommand CalculateCommand {  get; }
        public ICommand ClearCommand { get; }
        public ICommand NewDot { get; }
        public ICommand BackSpaceCommand { get; }
        public ICommand CEClearCommand { get; } 
        public ICommand ParenthesisCommand { get; }

        public ICommand ShowHistory { get; }
       

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            _isNewInput = true;
            EnterNumberCommand = new RelayCommand(EnterNumber);
            SetOpertaionCommand = new RelayCommand(SetOperation);
            CalculateCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            NewDot = new RelayCommand(GetNewDot);
            BackSpaceCommand = new RelayCommand(BackSpaceNumber);
            CEClearCommand = new RelayCommand(CEClear);
            ParenthesisCommand = new RelayCommand(Parenthesis);
            ShowHistory = new RelayCommand(History);



        }






        public List<string> history = new List<string>();
        

        private static readonly Dictionary<string, int> precedence = new Dictionary<string, int>//연산자에 우선순위를 부여하기 위해 딕셔너리사용
        {
            { "+", 1 }, { "-", 1 },
            { "*", 2 }, { "/", 2 } //만약 우선순위가 같을때, 먼저들어온게 pop되고 새로들어온게 push됨
        };

        // 중위식에서 하나씩 토큰으로 분리하는 작업
        private List<string> Tokenize(string expression) 
        {
            List<string> tokens = new List<string>();
            StringBuilder numberBuilder = new StringBuilder(); // 숫자와 연산자 분리를 위해 임시버퍼를 가진 StringBuilder를 선언

            foreach (char c in expression)//expression식 토큰화한걸 하나씩 검사
            {
                if (char.IsDigit(c) || c == '.') //숫자가 들어오면 임시버퍼에 바로 넣는다
                {
                    numberBuilder.Append(c);
                }
                else
                {
                    if (numberBuilder.Length > 0) //숫자가 있는데 연산자가 들어올 경우 
                    {
                        tokens.Add(numberBuilder.ToString());//있던 숫자는 tokens에 추가-> why? 후위표기법으로 바꾸려면 숫자와 연산자는 분리가 되어야 하기 때문
                        numberBuilder.Clear();//임시버퍼를 초기화해서  다음 숫자 받을 준비
                    }

                    if ("+-*/()".Contains(c))// 연산자나 괄호면
                    {
                        tokens.Add(c.ToString()); // tokens리스트에 추가
                    }
                }
            }

            if (numberBuilder.Length > 0) //숫자는 연산자가 아니라 마지막에 남아 있으니 마지막으로 남은 숫자를 토큰에 추가해야함.
            {
                tokens.Add(numberBuilder.ToString());
            }

            return tokens;
        }

        // 2) 중위식에서 후위식으로 바꾸는 함수
        private List<string> InfixToPostfix(string expression)
        {
            var tokens = Tokenize(expression);
            Stack<string> opStack = new Stack<string>();
            List<string> output = new List<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double num)) //이 함수에서 num은 token이 숫자인지 문자인지만 판별하는 변수라 굳이 활용은 안함.
                {
                    output.Add(token);
                }
                else if (token == "(")
                {
                    opStack.Push(token);
                }
                else if (token == ")")
                {
                    while (opStack.Count > 0 && opStack.Peek() != "(")
                        output.Add(opStack.Pop());
                    if (opStack.Count > 0) opStack.Pop();
                }
                else
                {
                    while (opStack.Count > 0 && opStack.Peek() != "(" &&   //여기서 연산자를 꺼내서 output리스트에 넣음.
                           precedence[opStack.Peek()] >= precedence[token])
                        output.Add(opStack.Pop());

                    opStack.Push(token);
                }
            }

            while (opStack.Count > 0)
                output.Add(opStack.Pop());

            return output;
        }

        // 후위식 계산 함수
        private double CalculatePostfix(List<string> postfixTokens) //reuturn으로 output리스트를 받아오게됨
        {
            Stack<double> finalStack = new Stack<double>();//최종 결과값을 계산하는 스택

            foreach (var token in postfixTokens)
            {
                if (double.TryParse(token, out double num)) // output리스트에 있는 것들을 스택에 넣음. 후위식 계산함수니까 이제 숫자는 바로 스택에 PUSH 해줌
                {
                    finalStack.Push(num);
                }
                else//연산자면 위에2개를 꺼냄
                {
                    double b = finalStack.Pop();
                    double a = finalStack.Pop();

                    switch (token)//2개 꺼낸걸 새로들어온 연산자를 이용해서 계산 
                    {
                        case "+": finalStack.Push(a + b); break;
                        case "-": finalStack.Push(a - b); break;
                        case "*": finalStack.Push(a * b); break;
                        case "/":
                            if (b == 0) throw new DivideByZeroException("0으로 나눌 수 없습니다.");
                            finalStack.Push(a / b);
                            break;
                    }
                }
            }

            return finalStack.Pop();// 최종계산해서 마지막 남은 결과값을 스택에서 pop
        }

        //전체 수식을 평가하는 함수
        private double StackCalculate(string expression)//만약 전체과정 함수를 안 쓴다면 Calculate함수에 이 과정을 다 넣어야 하기 때문에 유지보수와 가독성이 떨어짐.
        {
            var postfix = InfixToPostfix(expression);// 여기서 내부적으로 Tokenize를 했기때문에 이 함수에서는 호출x
            return CalculatePostfix(postfix);
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
                    //CalcExp = temp.ToString();


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

                if (string.IsNullOrEmpty(Display) || Display == "0" || _isNewInput)//Display가 비었거나 0이거나 새로운 입력이면 새로운 숫자로 교체
                {
                    Display = number; 
                    
                    _isNewInput = false;// 새로운 숫자가 있으면 초기화 안하고 이어붙이기
                }
                else
                {
                    Display += number;
                    //CalcExp += number;
                }
                
            }


        }

        private void Parenthesis(object parameter)

        {
            string parenthesis = parameter.ToString();

           
            if (string.IsNullOrEmpty(Display) || Display == "0") //Display가 비었거나 0 일때 괄호가 입력되면 괄호로 교체
            {
                Display = parenthesis; 


            }
            else {
                Display += parenthesis;

            }
            _isNewInput = false;
            //if (string.IsNullOrEmpty(CalcExp) || CalcExp == "0") 
            //{
            //    CalcExp = parenthesis;


            //}
            //else
            //{
            //    CalcExp += parenthesis;

            //}
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
            
            var tokens = Tokenize(Display);

            //  토큰 확인
            string lastToken = tokens.Count > 0 ? tokens[tokens.Count - 1] : "";

            // 마지막 토큰에 '.'이 없으면 점 추가
            if (!lastToken.Contains("."))
            {
                Display += ".";
            }
        }

        private void SetOperation(object parameter)
        {
            string op = parameter.ToString();

            // Display가 비었거나, 끝 문자가 연산자 또는 '(' 이면 연산자 추가가 안되도록 하기
            if (string.IsNullOrEmpty(Display)) return;

            char lastChar = Display[Display.Length - 1];

            if ("+-*/(".Contains(lastChar))
            {
                MessageBox.Show("잘못된 수식입니다.");
                return;
            }

            Display += op;
            //CalcExp += op;
            _isNewInput = false;

            _operation = op;  
        }


        private void Calculate(object parameter)
        {
            try
            {
                
                if (string.IsNullOrEmpty(_operation))
                    return;

                double result = StackCalculate(Display);

                if (result % 1 == 0)
                {
                    Display = ((int)result).ToString();
                    //CalcExp = ((int)result).ToString();
                }


                else {
                    Display = result.ToString("0.##");

                    _isNewInput = true;
                }

                history.Add(result.ToString());
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message);
                Clear(null);
            }
            catch (Exception)
            {
                MessageBox.Show("수식 오류입니다.");
                Clear(null);
            }
            
        }
        private void History(object parameter)
        {
            

        }
        private void Clear(object parameter)
        {
            
            Display = "0";
            //CalcExp = "0";
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
