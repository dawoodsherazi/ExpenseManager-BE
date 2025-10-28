namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string  Name { get; set; } = string.Empty;



        public int BookId { get; set; }
        public Book? Book { get; set; }



        public ICollection<Transaction>? Transactions { get; set; }
    }
}
