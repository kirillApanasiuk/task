using Microsoft.EntityFrameworkCore;
using playground.repositories.bookRepository.db.models;
namespace playground.repositories.bookRepository.db.providers
{
    public class SqliteProvider : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksAuthors>().HasKey(t => new { t.BookId, t.AuthorId });
            modelBuilder.Entity<BooksAuthors>().HasOne(sc => sc.Book).WithMany(s => s.BooksAuthors).HasForeignKey(sc => sc.BookId);
            modelBuilder.Entity<BooksAuthors>().HasOne(sc => sc.Author).WithMany(s => s.BooksAuthors).HasForeignKey(sc => sc.AuthorId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(
            /**/
            "Data Source=Sqlite.db"
        /**/
        );

        
    }
}