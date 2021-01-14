using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task_2
{
    public class NbuApiWorker
    {
        private readonly HttpClient _httpClient;
        public NbuApiWorker()
        {
            _httpClient = new HttpClient
            {
                Timeout = new TimeSpan(0, 0, 10)
            };
        }
        
        public async Task<string> UpdateExchangeRates()
        {
            var response = await _httpClient.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}