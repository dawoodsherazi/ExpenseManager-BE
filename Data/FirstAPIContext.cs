using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class FirstAPIContext : DbContext
    {
        public FirstAPIContext(DbContextOptions<FirstAPIContext> options)
        : base(options)
        { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Book>().HasData(

        //        new Book
        //        {
        //            Id = 1,
        //            Title = "Test",
        //            Author = "DAWOOD",
        //            Description = "Book 1",
        //            YearPublished = 1995
        //        },
        //     new Book
        //     {
        //         Id = 2,
        //         Title = "Test1",
        //         Author = "DAWOOD1",
        //         Description = "Book 2",
        //         YearPublished = 1996
        //     },
        //      new Book
        //      {
        //          Id = 3,
        //          Title = "Test1",
        //          Author = "DAWOOD1",
        //          Description = "Book 3",
        //          YearPublished = 1997
        //      }


        //        );
        //}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book → Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Categories)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category → Transaction (disable cascade to avoid multiple paths)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
