using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorPractice
{
    public class CalculatorModel
    {
        public double Add(double a, double b) => a + b;
        public double Substract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b == 0 ? throw new DivideByZeroException() : a / b;
        //public double Remainder(double a, double b) =>
        
       

    }
}
