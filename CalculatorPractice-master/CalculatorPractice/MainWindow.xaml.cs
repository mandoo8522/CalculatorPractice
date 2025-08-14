using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorPractice
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private CalculatorViewModel _viewmodel;
        public MainWindow()
        {
            _viewmodel = new CalculatorViewModel();
            InitializeComponent();
            DataContext = _viewmodel;
        }

        


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var vm = (CalculatorViewModel)DataContext;
            switch (e.Key)
            {
                
                case Key.D1: case Key.NumPad1: vm.EnterNumberCommand.Execute("1"); break;
                case Key.D2: case Key.NumPad2: vm.EnterNumberCommand.Execute("2"); break;
                case Key.D3: case Key.NumPad3: vm.EnterNumberCommand.Execute("3"); break;
                case Key.D4: case Key.NumPad4: vm.EnterNumberCommand.Execute("4"); break;
                case Key.D5: case Key.NumPad5: vm.EnterNumberCommand.Execute("5"); break;
                case Key.D6: case Key.NumPad6: vm.EnterNumberCommand.Execute("6"); break;
                
                
               

                


                case Key.OemMinus: vm.SetOpertaionCommand.Execute("-"); break;
                case Key.Multiply: vm.SetOpertaionCommand.Execute("*"); break;
                case Key.OemPlus: vm.SetOpertaionCommand.Execute("+"); break;
                case Key.OemQuestion: vm.SetOpertaionCommand.Execute("/"); break;


                case Key.Enter:
                    vm.CalculateCommand.Execute(null);  //=은 결과값을 출력하도록 하는 계산을 수행하기 때문에 Cal
                    break;
                case Key.OemPeriod: vm.NewDot.Execute("."); break;
                case Key.Back: vm.BackSpaceCommand.Execute(""); break;


                case Key.Escape: vm.ClearCommand.Execute("esc"); break;

                    }
            if (e.Key == Key.D8)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    vm.SetOpertaionCommand.Execute("*"); 
                }
                else
                {
                    vm.EnterNumberCommand.Execute("8");  
                }
            }
            
            if(e.Key==Key.D9)
            {
                if(Keyboard.IsKeyDown(Key.LeftShift))
                {
                    vm.ParenthesisCommand.Execute("(");
                }
                else
                {
                    vm.EnterNumberCommand.Execute("9");
                }
            }
            if (e.Key == Key.D0)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    vm.ParenthesisCommand.Execute(")");
                }
                else
                {
                    vm.EnterNumberCommand.Execute("0");
                }
            }


        }
        

    }
 


    }




