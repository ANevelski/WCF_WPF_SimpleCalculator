using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace WCF_CalculatorService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CalculatorService : ICalculatorService
    {
        public decimal CalculateValue(decimal value1, decimal value2, string operatorAction)
        {   
            decimal result = value2;
            switch (operatorAction)
            {
                case "+":
                    result = value1 + value2;
                    break;
                case "-":
                    result = value1 - value2;
                    break;
                case "×":
                    result = value1 * value2;
                    break;
                case "÷":
                    if (value2 != 0)
                    {
                        result = value1 / value2;
                    }
                    else
                    {
                        throw new InvalidOperationException("Division by zero is not allowed!");
                    }
                    break;
                case "%":
                    result = value1 % value2;
                    break;
            }           
            return result;
        }
    }
}
