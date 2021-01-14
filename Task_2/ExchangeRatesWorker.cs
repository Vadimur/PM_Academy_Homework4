using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Task_2
{
    public class ExchangeRatesWorker
    {
        private const string Path = "cache.json";
        private readonly NbuApiWorker _nbuApi;

        public ExchangeRatesWorker()
        {
            _nbuApi = new NbuApiWorker();
        }

        private void SaveExchangeRates(string content)
        {
            File.WriteAllText(Path, content);
        }
        
        public async Task<ExchangeRatesWorkerResult> ReadExchangeRates()
        {
            bool isRatesUpdated;
            try
            {
                string exchangeRatesJson = await _nbuApi.UpdateExchangeRates();
                SaveExchangeRates(exchangeRatesJson);
                isRatesUpdated = true;
            }
            catch (HttpRequestException)
            {
                isRatesUpdated = false;
            }
            catch (TaskCanceledException)
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
                return ExchangeRatesWorkerResult.Error($"The caller does not have the permission to open {Path} file");
            }
            catch (FileNotFoundException)
            {
                return ExchangeRatesWorkerResult.Error("Cannot fetch exchange rates");
            }
            catch (SecurityException)
            {
                return ExchangeRatesWorkerResult.Error($"Cannot open {Path} because the caller does not have the required permission.");
            }
            
            
            try
            {
                var exchangeRates = JsonConvert.DeserializeObject<List<ExchangeRate>>(content);
                
                if (isRatesUpdated)
                {
                    return new ExchangeRatesWorkerResult(true, exchangeRates, true);
                }
                else
                {
                    return new ExchangeRatesWorkerResult(true, exchangeRates, true, "Couldn't update exchange rates");
                }
            }
            catch (ArgumentNullException)
            {
                return ExchangeRatesWorkerResult.Error($"Cannot fetch exchange rates. Probably, {Path} is damaged");
            }
            catch (JsonException)
            {
                return ExchangeRatesWorkerResult.Error($"{Path} is damaged");
            }
            
        }
    }
}