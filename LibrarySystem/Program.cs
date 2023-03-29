using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using static LibrarySystem.Book;
using static LibrarySystem.User;

namespace LibrarySystem
{
    class Program
    {
        public static string BooksFileRoute = "C:/Users/leon.bonstrom/source/repos/LibrarySystem/LibrarySystem/BookFile.txt"; //används som en genväg för att spara värdena i string, för att kunna hittas av "Book.cs; och User.cs" sätts public framför 
        public static string UsersFileRoute = "C:/Users/leon.bonstrom/source/repos/LibrarySystem/LibrarySystem/UserFile.txt"; //Om man byter mappar och enhet behöves dessa länkar ändras
        static void Main(string[] args)
        {
            Console.Clear();

            //Lista över de skapade konton och böcker
            List<User> users = new List<User>();

            List<Book> books = new List<Book>();

            string[] BookLines = File.ReadAllLines(BooksFileRoute);
            foreach (var line in BookLines)
            {
                string[] Entries = line.Split(',');

                Book newBook = new Book() { Title = Entries[0], Author = Entries[1], ISBN = Entries[2], Subject = Entries[3], Available = bool.Parse(Entries[4]), ReservedBy = new List<User>() }; // Listans värden/information
                for (int i = 5; i < Entries.Length; i++)
                {
                    foreach (User aUser in users)
                    {
                        if (Entries[i] == aUser.PersonalNum)
                        {
                            newBook.ReservedBy.Add(aUser);
                        }
                    }
                }
                books.Add(newBook);
            }
            string[] UserLines = File.ReadAllLines(UsersFileRoute); //Läser av UserFile.txt och sedan seperar värdena med "," och lägger till dom i listan "users"
            foreach (var line in UserLines)
            {
                string[] Entries = line.Split(',');
                User newUser = new User() { PersonalNum = Entries[0], FirstName = Entries[1], LastName = Entries[2], Password = Entries[3], CurrentUser = bool.Parse(Entries[4]), Librarian = bool.Parse(Entries[5]), MyBooks = new List<Book>() };
                for (int i = 6; i < Entries.Length; i++) // Entries[6] och sedan uppåt är de böcker i MyBooks, böcker som användaren har lånat som då läggs till i listan
                {
                    foreach (Book aBook in books)
                    {
                        if (Entries[i] == aBook.ISBN) 
                        {
                            newUser.MyBooks.Add(aBook);
                        }
                    }
                }
                users.Add(newUser);
            }
            Menu(users, books); // Meny funktionen startas efter alla dessa värden har sätts in
        }
        static void Menu(List<User> users, List<Book> books) // Meny funktionen som kör hela programmet 
        {
            Console.Clear();
            foreach (Book aBook in books)
            {
                bool IsOwned = false;
                foreach (User aUser in users)
                {
                    foreach (Book thisBook in aUser.MyBooks)
                    {
                        if (aBook == thisBook)
                        {
                            IsOwned = true; // IsOwned blir true ifall den specifika boken ägs av en användare
                        }
                    }
                }
                if (IsOwned)
                {
                    aBook.Available = false; // boolen aBook.Available blir false om booken ägs av någon
                }
                else
                {
                    aBook.Available = true; //Om IsOwned är falsk blir aBook.Available True, alltså tillgänglig
                }
            }
            using (StreamWriter userWriter = new StreamWriter(UsersFileRoute)) // Här används StreamWriter för att spara värden i textfiler för användare och böckerna
            {
                foreach (User aUser in users)
                {
                    string UserWriting = $"{aUser}";
                    foreach (Book aBook in aUser.MyBooks)
                    {
                        UserWriting += $",{aBook.ISBN}"; //ISBN ska vara nummret som tår för att användaren äger boken
                    }
                    userWriter.WriteLine(UserWriting);
                }
            }
            using (StreamWriter bookWriter = new StreamWriter(BooksFileRoute))
            {
                foreach (Book aBook in books)
                {
                    string BookWriting = $"{aBook}";
                    foreach (User aUser in aBook.ReservedBy)
                    {
                        BookWriting += $",{aUser.PersonalNum}";
                    }
                    bookWriter.WriteLine(BookWriting);
                }
            }
            User CurrentUser2 = new User();
            bool LoggedIn = false;
            foreach (User aUser in users)
            {
                if (aUser.CurrentUser)
                {
                    CurrentUser2 = aUser;
                    LoggedIn = true;
                }
            }
            foreach (Book aBook in books)
            {
                if (aBook.Available)
                {
                    foreach (User aUser in users)
                    {
                        if (aBook.ReservedBy.Count >= 1)
                        {
                            if (aBook.ReservedBy[0].PersonalNum == aUser.PersonalNum)
                            {
                                aBook.Available = false;
                                aBook.ReservedBy.Remove(aUser);
                                aUser.MyBooks.Add(aBook);
                            }
                        }
                    }
                }
            }
            if (LoggedIn)
            {
                if (CurrentUser2.Librarian) // om man är inloggad som bibliotikarie
                {
                    LoggedIn = true;
                    Console.WriteLine("Välkommen, du är inloggad som bibliotikarie!");
                    Console.WriteLine("1) Konto");
                    Console.WriteLine("2) Böcker");
                    Console.WriteLine("3) Medlemar");
                    Console.WriteLine("4) Lägg till en bok");
                    Console.WriteLine("5) Sök efter böcker");
                    string MenuInput = Console.ReadLine();
                    switch (MenuInput.ToLower())
                    {
                        case "1":
                            Console.Clear();
                            users = ManageAccount(users, CurrentUser2); // fuktion där man kan ändra sitt konto
                            break;
                        case "2":
                            Console.Clear();
                            books = EditBook(books); // redigerar en bok
                            break;
                        case "3":
                            Console.Clear();
                            users = EditUser(users); // redigerar en användare
                            break;
                        case "4":
                            Console.Clear();
                            books = CreateBook(books); // Bibliotikarien skapar en bok
                            break;
                        case "5":
                            string BookSearch = Console.ReadLine();
                            List<Book> SearchedBooks = SearchBook(books, BookSearch); // Här söker bibliotikarien på en bok, där EditBook funktionen körs sedan
                            EditBook(SearchedBooks);
                            break;
                    }
                }
                else // inte är bibliotikarie men är inloggad
                {


                    Console.Clear();
                    Console.WriteLine($"Hej {CurrentUser2.FirstName} {CurrentUser2.LastName}!");
                    Console.WriteLine("Nu är du inloggad som medlem!");

                    Console.WriteLine("Vad vill du göra");
                    Console.WriteLine("1) Konto");
                    Console.WriteLine("2) Mina Böcker");
                    Console.WriteLine("3) Alla Böcker");
                    Console.WriteLine("4) Sök på böcker");

                    string MenuInput = Console.ReadLine();
                    switch (MenuInput)
                    {
                        case "1":
                            Console.Clear();
                            users = ManageAccount(users, CurrentUser2);
                            break;

                        case "2":
                            Console.Clear();
                            Console.WriteLine($"{CurrentUser2.FirstName}s böcker");
                            Book MyChosenbook = ChooseBooks(CurrentUser2.MyBooks);
                            if (MyChosenbook == null) { break; } // Om ChooseBook inte körs breakas switchen
                            Console.WriteLine(ViewBook(MyChosenbook)); // Skriver ut den valda bokens information
                            Console.WriteLine("1) Lämna tillbaka en bok");
                            Console.WriteLine("2) Tillbaka till föregående sida");
                            string MyBooksInput = Console.ReadLine();
                            switch (MyBooksInput.ToLower())
                            {
                                case "1":
                                    if (ReturnBook(MyChosenbook)) // Lämnar tillbaka boken och gör den tillgänglig, därmed raderar den från MyBooks
                                    {
                                        MyChosenbook.Available = true;
                                        CurrentUser2.MyBooks.Remove(MyChosenbook);
                                    }
                                    break;
                                case "2":
                                    Menu(users, books); 
                                    break;
                            }
                            break;
                        case "3":
                            Console.Clear();
                            Book ChosenBook = ChooseBooks(books);
                            if (ChosenBook == null) { break; } //Om ChooseBooks inte kan köras så breakas switchen
                            Console.WriteLine("Välj en book");
                            Console.WriteLine(ViewBook(ChosenBook)); // Skriver ut alla värden/informationen om boken
                            if (ChosenBook.Available)
                            {
                                Console.WriteLine("Vad vill du göra?");
                                Console.WriteLine("1) Låna bok");
                                Console.WriteLine("2) Gå tillbaka");
                                string BooksInput = Console.ReadLine();
                                switch (BooksInput.ToLower())
                                {
                                    case "1":
                                        if (BorrowBook(CurrentUser2.MyBooks, ChosenBook)) // Om det går att låna boken läggs den till i MyBooks och blir otillgänglig för andra 
                                        {
                                            ChosenBook.Available = false;
                                            CurrentUser2.MyBooks.Add(ChosenBook);
                                        }
                                        break;
                                    case "2":
                                        Menu(users, books);
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("1) Reservera en bok");
                                Console.WriteLine("2) Gå tillbaka till föregående sida");

                                string BookInput = Console.ReadLine();
                                switch (BookInput.ToLower())
                                {
                                    case "1":
                                        if (ReserveBook(CurrentUser2.MyBooks, ChosenBook)) //Reserverar en bok, om det fungerar läggs boken till i ReservedBy
                                        {
                                            ChosenBook.ReservedBy.Add(CurrentUser2);
                                        }
                                        break;
                                    case "2":
                                        Menu(users, books);
                                        break;
                                }
                            }
                            break;
                        case "4":
                            Console.Clear();
                            string BookSearch = Console.ReadLine();
                            List<Book> SearchedBooks = SearchBook(books, BookSearch);
                            Book SearchedChosenBook = ChooseBooks(SearchedBooks);
                            if (SearchedChosenBook == null) { break; } // Switch-satsen breakas ifall ChooseBook inte kan köras
                            Console.WriteLine(ViewBook(SearchedChosenBook));
                            Console.WriteLine("1) Låna bok");
                            Console.WriteLine("2) Gå tillbaka");
                            string SearchedBookInput = Console.ReadLine();
                            switch (SearchedBookInput)
                            {
                                case "1":
                                    if (BorrowBook(CurrentUser2.MyBooks, SearchedChosenBook)) //Lånar boken och gör boken otillgänglig för andra att låna den
                                    {
                                        SearchedChosenBook.Available = false;
                                        CurrentUser2.MyBooks.Add(SearchedChosenBook);
                                    }
                                    break;
                                case "2":
                                    Menu(users, books);
                                    break;
                            }
                            break;
                    }
                }
            }
            else // Ifall man inte är inloggad
            {
                Console.WriteLine("Välj vad du vill göra");
                Console.WriteLine("1) Logga in");
                Console.WriteLine("2) Registrera");
                Console.WriteLine("3) Böcker");
                Console.WriteLine("4) Sök på böcker");
                String Choice = Console.ReadLine();
                switch (Choice)
                {
                    case "1":
                        users = Login(users, books); // Loggar in på konto som finns i UserFile.txt
                        break;
                    case "2":
                        users = RegistrationPage(users); // skapar ett konto
                        break;
                    case "3":
                        Console.Clear();
                        ListAllBooks(books); // Kör ListAllBooks vilket är en funktion
                        break;
                    case "4":
                        Console.Clear();
                        string BookSearch = Console.ReadLine();
                        ListAllBooks(SearchBook(books, BookSearch)); //Söker på böckerna 
                        break;
                }
            }
            Console.ReadKey();
            Menu(users, books);
        }
    }
}
