using System.Transactions;

namespace WebApplication1.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; } = null!;

        public ICollection<Category>? Categories { get; set; }
        //public ICollection<Transaction>? Transaction { get; set; }
        //public string Description { get; set; } = null!;

        //public string Author { get; set; } = null!;

        //public int YearPublished { get; set; }

    }
}
