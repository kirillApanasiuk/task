using System.Collections.Generic;
namespace playground.repositories.bookRepository.db.models
{
    public class Author
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public List<BooksAuthors> BooksAuthors { get; set; }

        public Author()
        {
            BooksAuthors = new List<BooksAuthors>();
        }
    }
}