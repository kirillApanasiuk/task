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
            }
        }
        static void ShowWelcomeMenu()
        {
            Console.WriteLine("1.Add book");
            Console.WriteLine("2.Show all Authors");
            Console.WriteLine("3.Show all Books");
            Console.WriteLine("4.Show all authors books");
            Console.WriteLine("5.Show all authors books");
            Console.WriteLine("6.Delete book");

        }
    }
}
