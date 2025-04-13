using System;
using System.ServiceModel;

namespace CalculatorServiceHost
{
    internal class ServiceHostRun
    {
        static void Main(string[] args)
        {            
            using (var host = new ServiceHost(typeof(WCF_CalculatorService.CalculatorService)))
            {
                host.Open();
                Console.WriteLine("CalculatorService Host is started.");
                Console.ReadLine();
            }
        }
    }
}
