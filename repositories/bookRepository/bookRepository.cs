using System;
using playground.repositories.bookRepository.db;
class BooksRepository
{
    IBookRepository bookRepository;
    string storageMethod;
    public BooksRepository()
    {
        Console.WriteLine(Text.storageMethod);
        storageMethod = Console.ReadLine();
        if (storageMethod == "file")
        {
            bookRepository = new FileWorker();
        }
        if (storageMethod == "db")
        {
            bookRepository = new DataBaseWorker();
        }
        if (storageMethod != "file" && storageMethod != "db")
        {
            throw new System.ArgumentException(Text.errorStorageMethodSelect);
        }
    }
    public void AddBook(
        string book, string yearOfPublish, string[] authors
    )
    {
        if (authors.Length > 20)
        {
            Console.WriteLine(Text.validationBook);
            return;
        }
        foreach (string author in authors)
        {
            if (author.Length > 30)
            {
                Console.WriteLine(Text.validationAuthor);
                break;
            }
        }
        //validation
        bookRepository.AddBook(book, yearOfPublish, authors);
    }
    public void ShowAuthors()
    {
        bookRepository.ShowAuthors();
    }
    public void ShowBooks()
    {
        bookRepository.ShowBooks();
    }
    public void ShowAuthorsBooks(string authorId)
    {
        try
        {
            Int16.Parse(authorId);
            bookRepository.ShowAuthorsBooks(authorId);
        }
        catch (System.Exception)
        {
            Console.WriteLine(Text.invalidIndexAuthors);
        }
    }
    public void ShowBooksAuthors(string bookId)
    {
        try
        {
            Int16.Parse(bookId);
            bookRepository.ShowBooksAuthors(bookId);
        }
        catch (System.Exception)
        {
            Console.WriteLine(Text.invalidIndexBookInput);
        }
    }
    public void DeleteBook(string bookId)
    {
        try
        {
            Int16.Parse(bookId);
            bookRepository.DeleteBook(bookId);
        }
        catch (System.Exception)
        {
            Console.WriteLine(Text.invalidIndexBookInput);
        }
    }

}

interface IBookRepository
{
    void AddBook(string book, string yearOfPublish, string[] authors);
    void ShowAuthors();
    void ShowBooks();
    void ShowAuthorsBooks(string authorId);
    void ShowBooksAuthors(string bookId);
    void DeleteBook(string bookId);
}

interface BookRepository
{
    void AddBook(string book, string yearOfPublish, string[] authors);
    void ShowAuthors();
    void ShowBooks();
    void ShowAuthorsBooks(string authorId);
    void ShowBooksAuthors(string bookId);
    void DeleteBook(string bookId);
}