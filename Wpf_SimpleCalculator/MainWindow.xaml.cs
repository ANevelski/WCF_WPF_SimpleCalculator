using System;
using System.Windows;
using Wpf_SimpleCalculator.CalculatorService;


namespace Wpf_SimpleCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICalculatorProcessor _calculator;
        private CalculatorServiceClient _calcService;
        public MainWindow()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _calcService = new CalculatorServiceClient("BasicHttpBinding_ICalculatorService");
            _calculator = new CalculatorProcessor(_calcService);
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var number = (sender as System.Windows.Controls.Button).Content.ToString();                       

            ResultTextBox.Text =_calculator.AddNumberToValue(number, ResultTextBox.Text);

            if (_calculator.UpdateExpression())
            {
                ExpressionTextBox.Text = string.Empty;
                _calculator.LastValue = decimal.Parse(number);
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var operatorClicked = (sender as System.Windows.Controls.Button).Content.ToString();                     
            var lastValue = decimal.Parse(ResultTextBox.Text);

            _calculator.UpdatePropertiesForOperator(lastValue, operatorClicked);


            UpdateExpretionTextBox(_calculator.LastValue.ToString(), _calculator.CurrentOperator, _calculator.CurrentValue.ToString(), isEqua: false);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _calculator.ClearProperties();
           
            ResultTextBox.Text = "0";
            ExpressionTextBox.Text = string.Empty;
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (ResultTextBox.Text.Length > 1)
            {
                ResultTextBox.Text = ResultTextBox.Text.Substring(0, ResultTextBox.Text.Length - 1);
                if (string.IsNullOrEmpty(ExpressionTextBox.Text))
                {
                    _calculator.PreviousValue = _calculator.LastValue;
                    ResultTextBox.Text = _calculator.CurrentValue.ToString();
                }
            }
            else
            {
                _calculator.PreviousValue = _calculator.LastValue;
                _calculator.PreviousOperator = null;
                ResultTextBox.Text = "0";              
            }
            ExpressionTextBox.Text = string.Empty;
        }

        private void PlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(ResultTextBox.Text, out var val))
            {                
                ResultTextBox.Text = (-val).ToString();
            }
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            if (!ResultTextBox.Text.Contains("."))
            {
                ResultTextBox.Text += ".";
            }
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            decimal currentResult = decimal.Parse(ResultTextBox.Text);
              try
            {
                if (_calculator.PreviousOperator == null)
                {  
                    UpdateExpretionTextBox(_calculator.LastValue.ToString(), _calculator.CurrentOperator, currentResult.ToString(), isEqua: true);                   
                }
                else
                {
                    UpdateExpretionTextBox(_calculator.LastValue.ToString(), _calculator.CurrentOperator, _calculator.PreviousValue.ToString(), isEqua: true);
                }
                ResultTextBox.Text = _calculator.CalculateValue(currentResult);
            }
            catch (Exception ex)
            {
                _calculator.ClearProperties();
                ResultTextBox.Text = "0";
                ExpressionTextBox.Text = string.Empty;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);                
            }

        }      

        private void UpdateExpretionTextBox(string value, string operatorValue, string newVaue, bool isEqua = false)
        {
            if (isEqua) {
                if (string.IsNullOrEmpty(operatorValue))
                {
                    ExpressionTextBox.Text = string.Format("{0} =", newVaue);
                }
                else {
                    ExpressionTextBox.Text = string.Format("{0} {1} {2} =", value, operatorValue, newVaue);
                }
            }
            else
            {
                ExpressionTextBox.Text = string.Format("{0} {1}", value, operatorValue);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            calcService.Close();
        }
    }
}
