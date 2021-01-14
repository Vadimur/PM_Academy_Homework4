using System.Collections.Generic;

namespace Task_2
{
    public class ExchangeRatesReaderResult
    {
        public bool Success { get; }
        public List<ExchangeRate> ExchangeRates { get; }
        public bool IsUpdated { get; }
        public string ErrorMessage { get; }

        public ExchangeRatesReaderResult(bool success, List<ExchangeRate> exchangeRates, bool isUpdated, string errorMessage = null)
        {
            Success = success;
            ExchangeRates = exchangeRates;
            IsUpdated = isUpdated;
            ErrorMessage = errorMessage;
        }
        
        public static ExchangeRatesReaderResult Error(string errorMessage) => new ExchangeRatesReaderResult(false, null, false, errorMessage);

    }
}