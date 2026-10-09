namespace TMSWeb.Models;

public class AdvanceFundResult
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public long? TransactionId { get; set; }

    public decimal? Balance { get; set; }


    public static AdvanceFundResult Ok(
        string message,
        long? transactionId = null,
        decimal? balance = null)
    {
        return new AdvanceFundResult
        {
            Success = true,
            Message = message,
            TransactionId = transactionId,
            Balance = balance
        };
    }


    public static AdvanceFundResult Fail(
        string message)
    {
        return new AdvanceFundResult
        {
            Success = false,
            Message = message
        };
    }
}