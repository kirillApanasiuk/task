using System;
using System.IO;
using System.Collections.Generic;

namespace FileWorkerNameSpace
{
    public class FileWorker
    {


        private string[] storageFiles;
        public FileWorker()
        {
            storageFiles = new[] { Constants.books, Constants.authors, Constants.authorsAndBooks };
        }

        public void AddBook()
        {
            Console.WriteLine("Book name");
            string bookName = Console.ReadLine();
            Console.WriteLine("year");
            string year = Console.ReadLine();
            Console.WriteLine("Authors");
            string authorsString = Console.ReadLine();
            string[] authors = authorsString.Split(',');
            writeInfoToFiles(bookName, year, authors);

        }
        private void writeInfoToFiles(string book, string year, string[] authors)
        {
            int nextBookId = getNextIdOfFile(Constants.books);
            int nextAuthorsId = getNextIdOfFile(Constants.authors);
            int nextAuthorsAndBooksId = getNextIdOfFile(Constants.authorsAndBooks);
            List<int> AuthorIds = new List<int>();

            Console.WriteLine(nextBookId);

            foreach (string fileName in storageFiles)
            {
                using (StreamWriter fileStream = new StreamWriter(getPathToFile(fileName), true))
                {

                    if (fileName == Constants.books)
                    {
                        fileStream.WriteLine($"{nextBookId} {book} {year}");
                        ++nextBookId;
                    }
                    if (fileName == Constants.authors)
                    {
                        foreach (string author in authors)
                        {
                            int existingIndex = getExistingAuthorIndexFromFile(fileName, author);
                            if (existingIndex == 0)
                            {
                                fileStream.WriteLine($"{nextAuthorsId} {author}");
                                AuthorIds.Add(nextAuthorsId);
                                ++nextAuthorsId;
                            }
                            else
                            {
                                AuthorIds.Add(existingIndex);
                            }

                        }
                    }
                    if (fileName == Constants.authorsAndBooks)
                    {

                        foreach (int id in AuthorIds)
                        {
                            fileStream.WriteLine($"{nextAuthorsAndBooksId} {nextBookId - 1} {id}");
                            ++nextAuthorsAndBooksId;

                        }
                    }
                }
            }

        }

        private string getPathToFile(string fileName)
        {
            return $"{Directory.GetCurrentDirectory()}/storage/{fileName}";
        }
        private int getNextIdOfFile(string fileName)
        {
            if (!File.Exists(getPathToFile(fileName))) return 1;
            string lastLine = "";
            using (StreamReader sr = new StreamReader(getPathToFile(fileName)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lastLine = line;
                }

            }
            string[] lineInfo = lastLine.Split(' ');
            int lastIndex = Int16.Parse(lineInfo[0]);
            return lastIndex + 1;
        }

        private int getExistingAuthorIndexFromFile(string fileName, string author)
        {
            int index = 0;
            using (StreamReader sr = new StreamReader(getPathToFile(fileName)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(author))
                    {
                        string[] stringInfo = line.Split(' ');
                        index = Int16.Parse(stringInfo[0]);
                    }
                }

            }
            return index;
        }

    }
}


static class Constants
{
    public static string books = "books.txt";
    public static string authors = "authors.txt";
    public static string authorsAndBooks = "authorsAndBooks.txt";
}

