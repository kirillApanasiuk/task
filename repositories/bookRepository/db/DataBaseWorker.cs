using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using playground.repositories.bookRepository.db.providers;
using playground.repositories.bookRepository.db.models;
namespace playground.repositories.bookRepository.db
{
    public class DataBaseWorker : IBookRepository
    {
        public void AddBook(string book, string yearOfPublish, string[] authors)
        {
            using (SqliteProvider db = new SqliteProvider())
            {
                Book newBook = new Book { Title = $"{book}", YearOfPublish = $"{yearOfPublish}" };
                db.Books.Add(newBook);
                var authorsToSaveList = new List<Author>();
                var existingAuthorsIds = new List<int>();
                foreach (string authorLastName in authors)
                {
                    var author = db.Authors.Where(a => a.LastName == $"{authorLastName}").FirstOrDefault<Author>();
                    if (author != null)
                    {
                        existingAuthorsIds.Add(author.Id);
                        continue;
                    }
                    authorsToSaveList.Add(new Author { LastName = authorLastName });
                }
                db.Authors.AddRange(authorsToSaveList);
                db.SaveChanges();

                foreach (var authorIndex in existingAuthorsIds)
                {
                    newBook.BooksAuthors.Add(new BooksAuthors { BookId = newBook.Id, AuthorId = authorIndex });
                }

                for (int count = 0; count < authorsToSaveList.Count; count++)
                {

                    newBook.BooksAuthors.Add(new BooksAuthors { BookId = newBook.Id, AuthorId = authorsToSaveList.ElementAt(count).Id });
                }

                db.SaveChanges();
                Console.WriteLine(Text.successAddBook);
            }

        }
        public void ShowAuthors()
        {
            using (SqliteProvider db = new SqliteProvider())
            {
                Author[] authors = db.Authors.ToArray();
                foreach (var author in authors)
                {
                    Console.WriteLine($"Id - {author.Id},last name - {author.LastName} ");
                }
            }
        }

        public void ShowBooks()
        {
            using (SqliteProvider db = new SqliteProvider())
            {
                Book[] books = db.Books.ToArray();
                foreach (var book in books)
                {
                    Console.WriteLine($"Id - {book.Id},book title - {book.Title} ");
                }
            }
        }
        public void ShowAuthorsBooks(string authorId)
        {
            using (SqliteProvider db = new SqliteProvider())
            // {
            {
                // Author author = db.Authors.Single(a => a.Id == int.Parse(authorId)).BooksAuthors.Where(ba=>ba.AuthorId==)
                List<Book> books = db.Books.Where(b => b.BooksAuthors.Any(ba => ba.AuthorId == int.Parse(authorId))).ToList();
                if (books == null)
                {
                    Console.WriteLine(Text.invalidIndexAuthors);
                }
                foreach (var book in books)
                {
                    Console.WriteLine($"Id-{book.Id}, title - {book.Title}, year of publish -{book.YearOfPublish} ");
                }

            }
        }
        public void ShowBooksAuthors(string bookId)
        {
            using (SqliteProvider db = new SqliteProvider())
            // {
            {
                // Author author = db.Authors.Single(a => a.Id == int.Parse(authorId)).BooksAuthors.Where(ba=>ba.AuthorId==)
                List<Author> authors = db.Authors.Where(b => b.BooksAuthors.Any(ba => ba.BookId == int.Parse(bookId))).ToList();
                if (authors == null)
                {
                    Console.WriteLine(Text.invalidIndexAuthors);
                }
                foreach (var author in authors)
                {
                    Console.WriteLine($"Id-{author.Id}, last name - {author.LastName}");
                }
            }
        }
        public void DeleteBook(string bookId)
        {
            using (SqliteProvider db = new SqliteProvider())
            {
                db.Books.Remove(db.Books.Single(b => b.Id == int.Parse(bookId)));
                db.SaveChanges();
                List<Author> authors = db.Authors.Where(a => a.BooksAuthors.Any(ba => ba == null)).ToList();
                
            }
        }

    }
}