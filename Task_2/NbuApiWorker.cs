using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task_2
{
    public class ExchangeRatesUpdater
    {
        private readonly HttpClient httpClient = new HttpClient
        {
            Timeout = new TimeSpan(0, 0, 10)
        };

        private readonly ExchangeRatesReader fileWorker;

        public ExchangeRatesUpdater()
        {
            fileWorker = new ExchangeRatesReader();
        }
        private async Task UpdateExchangeRates()
        {
            var response = await httpClient.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            fileWorker.SaveExchangeRates(responseBody);
        }
    }
}