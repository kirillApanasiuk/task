using System.Collections.Generic;
namespace playground.repositories.bookRepository.db.models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string YearOfPublish { get; set; }
        public List<BooksAuthors> BooksAuthors { get; set; }
        public Book()
        {
            BooksAuthors = new List<BooksAuthors>();
        }


    }
}