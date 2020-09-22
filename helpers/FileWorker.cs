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
        public void ShowAuthors()
        {
            readAndLogFile(getPathToFile(Constants.authors), "No authors yet");
        }
        public void ShowBooks()
        {
            readAndLogFile(getPathToFile(Constants.books), "No books yet");
        }

        public void ShowAuthorBooks(string authorIdAsString)
        {
            if (!File.Exists(getPathToFile(Constants.authors)))
            {
                Console.WriteLine("No authors yet");
                return;
            }
            List<string> booksIds = getIndexesOfAllBooksOfTheAuthor(authorIdAsString);
            showSelectedBooks(booksIds);

        }
        public void ShowBookAuthor(string bookIdAsString) { }

        public void DeleteBook(string bookIdAsString)
        {

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

                        foreach (int authorId in AuthorIds)
                        {
                            fileStream.WriteLine($"{nextAuthorsAndBooksId} {nextBookId - 1} {authorId}");
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
        private void readAndLogFile(string filePath, string noInfoMessage = "")
        {

            if (!File.Exists(filePath))
            {
                Console.WriteLine(noInfoMessage);
                return;


            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }

            }
        }
        private List<string> getIndexesOfAllBooksOfTheAuthor(string authorIdAsString)
        {
            List<string> booksIdsAsString = new List<string>();
            using (StreamReader sr = new StreamReader(getPathToFile(Constants.authorsAndBooks)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] LineInfo = line.Split(' ');
                    if (LineInfo[2] == authorIdAsString)
                    {
                        booksIdsAsString.Add(LineInfo[1]);
                    }
                }
            }
            return booksIdsAsString;
        }

        private void showSelectedBooks(List<string> booksIds)
        {
            using (StreamReader sr = new StreamReader(getPathToFile(Constants.books)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] LineInfo = line.Split(' ');
                    foreach (string bookId in booksIds)
                    {
                        if (bookId == LineInfo[0]) Console.WriteLine(line);
                    }
                }

            }
        }
    }
}


static class Constants
{
    public static string books = "books.txt";
    public static string authors = "authors.txt";
    public static string authorsAndBooks = "authorsAndBooks.txt";
}

