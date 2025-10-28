namespace WebApplication1.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public decimal  Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;



        public int CategoryId { get; set; }
        public Category? Category { get; set; }




    }
}
