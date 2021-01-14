using Newtonsoft.Json;

namespace Task_2
{
    public class ExchangeRate
    {
        [JsonProperty("r030")]
        public string ExchangeRateCode { get; set; }
        
        [JsonProperty("txt")]
        public string CurrencyName { get; set; }
        
        [JsonProperty("rate")]
        public decimal Rate { get; set; }
        
        [JsonProperty("cc")]
        public string CurrencyCode { get; set; }
        
        [JsonProperty("exchangedate")]
        public string ExchangeDate { get; set; }
    }
}