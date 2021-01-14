using System.Collections.Generic;

namespace Task_2
{
    public class ExchangeRatesWorkerResult
    {
        public bool Success { get; }
        public List<ExchangeRate> ExchangeRates { get; }
        public bool IsUpdated { get; }
        public string Message { get; }

        public ExchangeRatesWorkerResult(bool success, List<ExchangeRate> exchangeRates, bool isUpdated, string message = null)
        {
            Success = success;
            ExchangeRates = exchangeRates;
            IsUpdated = isUpdated;
            Message = message;
        }
        
        public static ExchangeRatesWorkerResult Error(string message) => new ExchangeRatesWorkerResult(false, null, false, message);

    }
}