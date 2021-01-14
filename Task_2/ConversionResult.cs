namespace Task_2
{
    public class ConvertionResult
    {
        public bool Success { get; }
        public string Message { get; }
        public decimal MoneyAmount { get; }
        public string Date { get; }
        public decimal Rate { get; }

        public ConvertionResult(bool success, decimal moneyAmount, string date, decimal rate, string message = null)
        {
            MoneyAmount = moneyAmount;
            Date = date;
            Rate = rate;
            Success = success;
            Message = message;
        }

        public static ConvertionResult Error(string errorMessage) => new ConvertionResult(false, 0, null, 0, errorMessage);
    }
}