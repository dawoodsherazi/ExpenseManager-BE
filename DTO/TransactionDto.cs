namespace WebApplication1.DTO
{
    public class TransactionDto
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;

        public int CategoryId { get; set; }
    }
}
