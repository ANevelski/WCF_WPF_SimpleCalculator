using System;
using Wpf_SimpleCalculator.CalculatorService;

public interface ICalculatorProcessor
{    decimal CurrentValue { get; set; }
    decimal LastValue { get; set; }
    decimal PreviousValue { get; set; }
    string CurrentOperator { get; set; }
    string PreviousOperator { get; set; }
    bool IsOperatorPressed { get; set; }
    void ClearProperties();
    void UpdatePropertiesForOperator(decimal lastValue, string currentOperator);

    string AddNumberToValue(string number, string value);
    bool UpdateExpression();
    string CalculateValue(decimal currentResult);
}

public class CalculatorProcessor : ICalculatorProcessor
{
    public decimal CurrentValue { get; set; }
    public decimal LastValue { get; set; }
    public decimal PreviousValue { get; set; }
    public string CurrentOperator { get; set; }
    public string PreviousOperator { get; set; }
    public bool IsOperatorPressed { get; set; }

    private CalculatorServiceClient  _calcService;

    public CalculatorProcessor (CalculatorServiceClient calcService)
    {
        _calcService = calcService;
    }

    public void ClearProperties()
    {
        CurrentValue = 0;
        LastValue = 0;
        CurrentOperator = null;
        PreviousOperator = null;
        PreviousValue = 0;        
    }

    public void UpdatePropertiesForOperator(decimal lastValue, string currentOperator)
    {
        LastValue = lastValue;
        CurrentOperator = currentOperator;
        IsOperatorPressed = true;
        PreviousOperator = null;
    }

    public bool UpdateExpression()
    {
        return PreviousOperator != null && PreviousOperator.Equals("=");
    }

    public string AddNumberToValue(string number, string value)
    {
        string result = value;
        if ((IsOperatorPressed || value == "0") && value != "0.")
        {
            result = number;
            IsOperatorPressed = false;
        }
        else
        {
            result += number;
        }

        return result;
    }

    public string CalculateValue(decimal currentResult)
    {
        string calculatedValue;
        try
        {
            if (PreviousOperator == null)
            {
                calculatedValue = CalculateValue(LastValue, currentResult, CurrentOperator);

                PreviousOperator = "=";
                PreviousValue = currentResult;
            }
            else
            {
                calculatedValue = CalculateValue(LastValue, PreviousValue, CurrentOperator);
            }

            IsOperatorPressed = true;
            CurrentValue = currentResult;
            LastValue = decimal.Parse(calculatedValue);
        }
        catch(Exception ex)
        {
            throw ex;
        }

        return calculatedValue;
    }

    private string CalculateValue(decimal value, decimal newValue, string operatorAction)
    {
        if (newValue == 0 && operatorAction.Equals("÷"))
            throw new InvalidOperationException("Division by zero is not allowed!");

        return _calcService.CalculateValue(value, newValue, operatorAction).ToString();
    }
}
