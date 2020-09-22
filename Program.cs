using System;
using Methods;
using FileWorkerNameSpace;

namespace playground
{
    class Program
    {
        static void Main(string[] args)
        {
            MyMethods methods = new MyMethods();
            FileWorker fileWorker = new FileWorker();
            Console.WriteLine("Welcome to Book tracker");
            Console.WriteLine("Enter the number of the selected action");
            string currentUserInput;
            while (true)
            {
                ShowWelcomeMenu();
                currentUserInput = Console.ReadLine();
                if (currentUserInput == "1")
                {
                    fileWorker.AddBook();
                }
                if (currentUserInput == "2")
                {
                    fileWorker.ShowAuthors();
                }
                if (currentUserInput == "3")
                {
                    fileWorker.ShowBooks();
                }
                if (currentUserInput == "4")
                {
                    fileWorker.ShowAuthors();
                    fileWorker.ShowAuthorBooks(Console.ReadLine());
                }
                if (currentUserInput == "5")
                {
                    fileWorker.ShowBooks();
                    fileWorker.ShowBookAuthors(Console.ReadLine());
                }
                if (currentUserInput == "6")
                {
                    fileWorker.ShowBooks();
                    Console.WriteLine("Enter the index of book which you wish to delete");
                    fileWorker.DeleteBook(Console.ReadLine());
                }
            }

        }
        static void ShowWelcomeMenu()
        {
            Console.WriteLine("1.Add book");
            Console.WriteLine("2.Show all Authors");
            Console.WriteLine("3.Show all Books");
            Console.WriteLine("4.Show selected author books");
            Console.WriteLine("5.Show selected book authors");
            Console.WriteLine("6.Delete book");

        }
    }
}
