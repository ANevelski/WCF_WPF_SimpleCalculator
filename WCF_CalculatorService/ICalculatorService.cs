using System.ServiceModel;

namespace WCF_CalculatorService
{  
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        decimal CalculateValue(decimal value, decimal newValue, string operatorAction);
    }
}