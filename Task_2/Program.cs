using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Task_2
{
    class Program
    {
        private static string author = "Made by Mulish Vadym\n";
        private static string programDescription = "Task 2 Currency Converter";
        
        static async Task Main(string[] args)
        {
            CurrencyConverter converter = new CurrencyConverter();
            
            Console.WriteLine(programDescription);
            Console.WriteLine(author);

            Console.Write("Enter the initial currency: ");
            string fromCurrency = ReadCurrency();
            
            Console.Write("Enter the currency to which to convert money: ");
            string toCurrency = ReadCurrency();
            
            Console.Write("Enter money amount: ");
            decimal moneyAmount = ReadMoneyAmount();

            ConversionResult result = await converter.MakeConversion(fromCurrency, toCurrency, moneyAmount);
            
            Console.WriteLine();

            if (!result.Success)
            { 
                Console.WriteLine($"Error occured: {result.Message}");
            }
            else
            {
                if (result.Message != null)
                {
                    Console.WriteLine(result.Message);
                }
                Console.WriteLine($"{moneyAmount} {fromCurrency} = {result.MoneyAmount} {toCurrency}\nDate: {result.Date}\nExchange rate: {result.Rate}");
            }

            Console.Read();
        }

        private static string ReadCurrency()
        { 
            while (true)
            {
                string rawCurrency = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(rawCurrency) || rawCurrency.Trim().Length != 3)
                {
                    Console.WriteLine("Try again...");
                    continue;
                }

                return rawCurrency.Trim().ToUpper();
            }
        }
        
        private static decimal ReadMoneyAmount()
        { 
            while (true)
            {
                string rawMoneyAmount = Console.ReadLine()?.Replace(",", ".");
                
                if (string.IsNullOrWhiteSpace(rawMoneyAmount) 
                    || !decimal.TryParse(rawMoneyAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal moneyAmount)
                    || moneyAmount < 0)
                {
                    Console.WriteLine("Invalid amount of money");
                    continue;
                }
                
                return moneyAmount;
            }
        }
    }
}
