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
                case Key.D0: case Key.NumPad0: vm.EnterNumberCommand.Execute("0"); break;
                case Key.D1: case Key.NumPad1: vm.EnterNumberCommand.Execute("1"); break;
                case Key.D2: case Key.NumPad2: vm.EnterNumberCommand.Execute("2"); break;
                case Key.D3: case Key.NumPad3: vm.EnterNumberCommand.Execute("3"); break;
                case Key.D4: case Key.NumPad4: vm.EnterNumberCommand.Execute("4"); break;
                case Key.D5: case Key.NumPad5: vm.EnterNumberCommand.Execute("5"); break;
                case Key.D6: case Key.NumPad6: vm.EnterNumberCommand.Execute("6"); break;
                case Key.D7: case Key.NumPad7: vm.EnterNumberCommand.Execute("7"); break;
                
               

                case Key.D9: case Key.NumPad9: vm.EnterNumberCommand.Execute("9"); break;


                case Key.OemMinus: vm.SetOpertaionCommand.Execute("-"); break;
                case Key.Multiply: vm.SetOpertaionCommand.Execute("*"); break;
                case Key.OemPlus: vm.SetOpertaionCommand.Execute("+"); break;
                case Key.OemQuestion: vm.SetOpertaionCommand.Execute("/"); break;
                

                case Key.Enter: vm.SetOpertaionCommand.Execute("="); break;
                case Key.OemPeriod: vm.NewDot.Execute("."); break;
                case Key.Back: vm.BackSpaceCommand.Execute(""); break;




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
            else if (e.Key == Key.NumPad8)
            {
                vm.EnterNumberCommand.Execute("8"); 
            }

        }
        

    }
 


    }




