using System;
using System.IO;
using System.Collections.Generic;


class FileWorker : IBookRepository
{


    private string[] storageFiles;
    public FileWorker()
    {
        storageFiles = new[] { Constants.books, Constants.authors, Constants.authorsAndBooks };
    }

    public void AddBook(string book, string year, string[] authors)
    {
        writeInfoToFiles(book, year, authors);
    }
    public void ShowAuthors()
    {
        readAndLogFile(getPathToFile(Constants.authors), Text.errorFileNoAuthorsFile);
    }
    public void ShowBooks()
    {
        readAndLogFile(getPathToFile(Constants.books), Text.errorFileNoBooksFile);
    }

    public void ShowAuthorsBooks(string authorIdAsString)
    {
        if (!File.Exists(getPathToFile(Constants.authors)))
        {
            Console.WriteLine("No authors yet");
            return;
        }
        List<string> booksIds = getIndexesOfAllBooksOfTheAuthor(authorIdAsString);
        if (booksIds.Count == 0)
        {
            Console.WriteLine("You index is invalid");
            return;
        }
        showSelectedFileLines(booksIds, getPathToFile(Constants.books));

    }
    public void ShowBooksAuthors(string bookIdAsString)
    {
        if (!File.Exists(getPathToFile(Constants.books)))
        {
            Console.WriteLine(Text.errorFileNoBooksFile);
            return;
        }
        List<string> authorsIds = getIndexesOfAllAuthorsOfABook(bookIdAsString);
        if (authorsIds.Count == 0)
        {
            Console.WriteLine(Text.invalidIndexAuthors);
            return;
        }
        showSelectedFileLines(authorsIds, getPathToFile(Constants.authors));
    }

    public void DeleteBook(string bookIdAsString)
    {
        if (!isFilesReady())
        {
            Console.WriteLine(Text.errorFileFilesUnready);
            return;
        }
        deleteBookFromBooks(bookIdAsString);
        List<string> deletedBooksAuthorsIds = deleteAllBookRelationsAndGetDeletedAuthorsIds(bookIdAsString);
        foreach (var authorId in deletedBooksAuthorsIds)
        {
            deleteTheAuthorIfHeDidNotWriteTheBook(authorId);
        }

    }


    private void writeInfoToFiles(string book, string year, string[] authors)
    {
        int nextBookId = getNextIdOfFile(Constants.books);
        int nextAuthorsId = getNextIdOfFile(Constants.authors);
        int nextAuthorsAndBooksId = getNextIdOfFile(Constants.authorsAndBooks);
        List<int> AuthorIds = new List<int>();
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

    private List<string> getIndexesOfAllAuthorsOfABook(string bookIdAsString)
    {
        List<string> authorsIdsAsString = new List<string>();
        using (StreamReader sr = new StreamReader(getPathToFile(Constants.authorsAndBooks)))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                if (LineInfo[1] == bookIdAsString)
                {
                    authorsIdsAsString.Add(LineInfo[2]);
                }
            }
        }
        return authorsIdsAsString;
    }

    private void showSelectedFileLines(List<string> ids, string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                foreach (string id in ids)
                {
                    if (id == LineInfo[0]) Console.WriteLine(line);
                }
            }

        }

    }
    private bool isFilesReady()
    {
        bool isOk = true;
        foreach (var fileName in storageFiles)
        {
            if (!File.Exists(getPathToFile(fileName)))
            {

                Console.WriteLine($"File not exist {fileName}");
                isOk = false;
                break;
            }
        }
        return isOk;
    }
    private void deleteBookFromBooks(string bookIdAsString)
    {
        using (StreamReader sr = new StreamReader(getPathToFile(Constants.books)))
        using (StreamWriter sw = new StreamWriter(getPathToFile(Constants.temp)))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                if (LineInfo[0] != bookIdAsString)
                {
                    sw.WriteLine(line);
                }
            }

        }
        overwritingFiles(getPathToFile(Constants.books), getPathToFile(Constants.temp));
    }

    private List<string> deleteAllBookRelationsAndGetDeletedAuthorsIds(string bookIdAsString)
    {
        List<string> authorsIds = new List<string>();
        using (StreamReader sr = new StreamReader(getPathToFile(Constants.authorsAndBooks)))
        using (StreamWriter sw = new StreamWriter(getPathToFile(Constants.temp)))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                if (LineInfo[1] == bookIdAsString)
                {
                    authorsIds.Add(LineInfo[2]);

                }
                else
                {
                    sw.WriteLine(line);
                }
            }

        }
        overwritingFiles(getPathToFile(Constants.authorsAndBooks), getPathToFile(Constants.temp));
        return authorsIds;
    }

    private void deleteTheAuthorIfHeDidNotWriteTheBook(string authorIdAsString)
    {
        using (StreamReader sr = new StreamReader(getPathToFile(Constants.authorsAndBooks)))
        {
            bool authorExist = false;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                if (LineInfo[2] == authorIdAsString)
                {
                    authorExist = true;
                    break;
                }
            }
            if (!authorExist) deleteAuthorFromAuthors(authorIdAsString);
        }
    }

    private void deleteAuthorFromAuthors(string authorIdAsString)
    {

        using (StreamReader sr = new StreamReader(getPathToFile(Constants.authors)))
        using (StreamWriter sw = new StreamWriter(getPathToFile(Constants.temp)))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] LineInfo = line.Split(' ');
                if (LineInfo[0] != authorIdAsString)
                {
                    sw.WriteLine(line);
                }
            }

        }
        overwritingFiles(getPathToFile(Constants.authors), getPathToFile(Constants.temp));
    }
    private void overwritingFiles(string pathToFile, string pathToTempFile)
    {
        File.Delete(pathToFile);
        File.Move(pathToTempFile, pathToFile);
        File.Delete(pathToTempFile);
    }
}



static class Constants
{
    public static string books = "books.txt";
    public static string authors = "authors.txt";
    public static string authorsAndBooks = "authorsAndBooks.txt";
    public static string temp = "temp.txt";
}

