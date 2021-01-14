using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_2
{
    public class CurrencyConverter
    {
        private readonly ExchangeRatesWorker _exchangeRatesReader;
        private List<ExchangeRate> _exchangeRates;

        public CurrencyConverter()
        {
            _exchangeRatesReader = new ExchangeRatesWorker();
            
        }
        public async Task<ConversionResult> MakeConversion(string fromCurrency, string toCurrency, decimal amount)
        {
            var exchangeWorkerResult = await _exchangeRatesReader.ReadExchangeRates();
            if (!exchangeWorkerResult.Success)
            {
                return ConversionResult.Error(exchangeWorkerResult.Message);
            }
            
            _exchangeRates = exchangeWorkerResult.ExchangeRates;
            if (_exchangeRates == null || _exchangeRates.Count == 0)
            {
                return ConversionResult.Error("Cannot fetch exchange rates");
            }
            
            ExchangeRate toCurrencyExchangeRate = FindExchangeRate(toCurrency);
            if (toCurrencyExchangeRate == null && !toCurrency.Equals("UAH"))
            {
                return ConversionResult.Error($"Currency {toCurrency} is not supported!");
            }
            
            ExchangeRate fromCurrencyExchangeRate = FindExchangeRate(fromCurrency);
            if (fromCurrencyExchangeRate == null && !fromCurrency.Equals("UAH"))
            {
                return ConversionResult.Error($"Currency {fromCurrency} is not supported!");
            }

            if (fromCurrency.Equals("UAH") && toCurrency.Equals("UAH"))
            {
                return new ConversionResult(true, amount, DateTime.Today.ToShortDateString(), 1);
            }
            if (fromCurrency.Equals("UAH"))
            {
                return ConvertFromUAH(toCurrencyExchangeRate, amount, exchangeWorkerResult.Message);
            }
            if (toCurrency.Equals("UAH"))
            {
                return ConvertToUAH(fromCurrencyExchangeRate, amount, exchangeWorkerResult.Message);
            }

            if (toCurrencyExchangeRate == null || fromCurrencyExchangeRate == null)
                return ConversionResult.Error("Error occured");
            
            decimal exchangeRateDoubleConversion = Math.Round(fromCurrencyExchangeRate.Rate / toCurrencyExchangeRate.Rate, 2);
            decimal convertedMoneyAmount = Math.Round(exchangeRateDoubleConversion * amount, 2);
            
            return new ConversionResult(true, convertedMoneyAmount, toCurrencyExchangeRate.ExchangeDate, exchangeRateDoubleConversion, exchangeWorkerResult.Message);
        }

        private ExchangeRate FindExchangeRate(string currencyName)
        {
            var exchangeRate = _exchangeRates.FirstOrDefault(c => c.CurrencyCode.Equals(currencyName));
            return exchangeRate;
        }
        
        private ConversionResult ConvertFromUAH(ExchangeRate toCurrencyExchangeRate, decimal amount, 
            string message = null)
        {
            decimal moneyAmount = Math.Round(amount / toCurrencyExchangeRate.Rate, 2);
            string exchangeDate = toCurrencyExchangeRate.ExchangeDate;
            decimal exchangeRate = toCurrencyExchangeRate.Rate;
                
            return new ConversionResult(true, moneyAmount, exchangeDate, exchangeRate, message);
        }
        
        private ConversionResult ConvertToUAH(ExchangeRate fromCurrencyExchangeRate, decimal amount,
            string message = null)
        {
            decimal moneyAmount = Math.Round(amount * fromCurrencyExchangeRate.Rate, 2);
            string exchangeDate = fromCurrencyExchangeRate.ExchangeDate;
            decimal exchangeRate = fromCurrencyExchangeRate.Rate;
            
            return new ConversionResult(true, moneyAmount, exchangeDate, exchangeRate, message);
        }
        
    }
}