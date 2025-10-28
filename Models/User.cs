

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

       
        public ICollection<Book>? Books { get; set; }
    }
}
