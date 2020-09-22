using System;
namespace playground
{
    class Program
    {
        static void Main(string[] args)
        {

            BooksRepository bookRepository = new BooksRepository();
            Console.WriteLine(Text.appWelcomeMessage);
            Console.WriteLine(Text.invitationToTheMainAction);
            string currentUserInput;
            while (true)
            {
                ShowWelcomeMenu();
                currentUserInput = Console.ReadLine();
                if (currentUserInput == "1")
                {
                    Console.WriteLine(Text.requestForTheBookInput);
                    string bookName = Console.ReadLine();
                    Console.WriteLine(Text.requestForTheYearOfBookPublishInput);
                    string year = Console.ReadLine();
                    Console.WriteLine(Text.requestForTheAuthorsInput);
                    string authorsString = Console.ReadLine();
                    string[] authors = authorsString.Split(',');
                    bookRepository.AddBook(bookName, year, authors);
                }
                if (currentUserInput == "2")
                {
                    bookRepository.ShowAuthors();
                }
                if (currentUserInput == "3")
                {
                    bookRepository.ShowBooks();
                }
                if (currentUserInput == "4")
                {
                    bookRepository.ShowAuthors();
                    bookRepository.ShowAuthorsBooks(Console.ReadLine());
                }
                if (currentUserInput == "5")
                {
                    bookRepository.ShowBooks();
                    bookRepository.ShowBooksAuthors(Console.ReadLine());
                }
                if (currentUserInput == "6")
                {
                    bookRepository.ShowBooks();
                    Console.WriteLine(Text.requestFortheBookIndexForDelete);
                    bookRepository.DeleteBook(Console.ReadLine());
                }
            }

        }
        static void ShowWelcomeMenu()
        {
            Console.WriteLine(Text.mainMenuAddBook);
            Console.WriteLine(Text.mainMenuShowAllAuthors);
            Console.WriteLine(Text.mainMenuShowAllBooks);
            Console.WriteLine(Text.mainMenuShowSelectedAuthorBooks);
            Console.WriteLine(Text.mainMenuShowSelectedBookAuthors);
            Console.WriteLine(Text.mainMenuDeleteBook);

        }
    }
}
