using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Task_2
{
    public class ExchangeRatesReader
    {
        private const string Path = "cache.json";
        private readonly NbuApiWorker _nbuApi;

        public ExchangeRatesReader()
        {
            _nbuApi = new NbuApiWorker();
        }
        public void SaveExchangeRates(string content)
        {
            File.WriteAllText(Path, content);
        }
        
        public async Task<ExchangeRatesReaderResult> ReadExchangeRates()
        {
            bool isRatesUpdated;
            try
            {
                await _nbuApi.UpdateExchangeRates();
                isRatesUpdated = true;
            }
            catch (HttpRequestException e)
            {
                isRatesUpdated = false;
            }

            string content;
            try
            {
                content = await File.ReadAllTextAsync(Path);
            }
            catch (UnauthorizedAccessException)
            {
                return ExchangeRatesReaderResult.Error("The caller does not have the required permission.");
            }
            catch (FileNotFoundException)
            {
                return ExchangeRatesReaderResult.Error("Cannot fetch exchange rates");
            }
            catch (SecurityException)
            {
                return ExchangeRatesReaderResult.Error("The caller does not have the required permission.");
            }
            
            
            try
            {
                var exchangeRates = JsonConvert.DeserializeObject<List<ExchangeRate>>(content);
                if (isRatesUpdated)
                {
                    return new ExchangeRatesReaderResult(true, exchangeRates, true);
                }
                else
                {
                    return  ExchangeRatesReaderResult.Error("Couldn't update exchange rates");
                }
            }
            catch (ArgumentNullException)
            {
                return  ExchangeRatesReaderResult.Error("Cannot fetch exchange rates");
            }
            catch (JsonException)
            {
                return  ExchangeRatesReaderResult.Error("The JSON is invalid");
            }
            
        }
    }
}