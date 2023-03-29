using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace LibrarySystem
{
    class Book
    {
        // En klass som skapar strängar för böckerna 

        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Subject { get; set; }
        public List<User> ReservedBy { get; set; }
        public bool Available { get; set; }


        public override string ToString()
        {
            return $"{Title},{Author},{ISBN},{Subject},{Available}";
        }
        public static bool BorrowBook(List<Book> MyBooks, Book ChosenBook)
        {
            bool ChosenBookSuccess = true;
            foreach (Book UrBook in MyBooks)
            {
                if (ChosenBook.ISBN == UrBook.ISBN)
                {
                    ChosenBookSuccess = false;
                    Console.WriteLine("Du har redan lånat den här book");
                    break;
                }
            }
            if (ChosenBookSuccess)
            {
                if (ChosenBook.Available)
                {
                    Console.WriteLine($"Vill du låna {ChosenBook.Title} skriven av {ChosenBook.Author} (y/n)");
                    string ConfirmBorrow = Console.ReadLine();

                    if (ConfirmBorrow == "y")
                    {
                        Console.WriteLine($"Nu har du lånat {ChosenBook.Title}");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Boken är int etillgänglig att låna tyvärr");
                }
            }
            return false;
        }
        public static Book ChooseBooks(List<Book> books)
        {
            ListAllBooks(books); // Listar böckerna och sorterar dom efter författare
            var sort = from aBook in books
                       orderby aBook.Author
                       select aBook;
            try
            {
                int ChosenBook = int.Parse(Console.ReadLine());
                int BookNum = 1;
                bool ChosenBookSuccess = false; 
                foreach (var aBook in sort) //
                {
                    if (ChosenBook == BookNum)
                    {
                        ChosenBookSuccess = true;
                        return aBook;
                    }
                    BookNum++;
                }
                if (ChosenBookSuccess == false)
                {
                    Console.WriteLine("Boken du har skrivit in är ej tillgänglig");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return null;
        }
        public static bool ReturnBook(Book Chosenbook) //funktion för att kunna lämna tillbaka en bok
        {
            Console.WriteLine($"Säker på att du vill lämna tillbaka denna?:{Chosenbook.Title} skriven av {Chosenbook.Author} (y/n)");
            string ConfirmReturn = Console.ReadLine();
            if (ConfirmReturn.ToLower() == "y")
            {
                return true;
            }
            return false;
        }
        public static bool ReserveBook(List<Book> MyBooks, Book ChosenBook) // funktion för att reservera en bok
        {
            bool ChosenBookSuccess = true;
            foreach (Book UrBook in MyBooks)
            {
                if (ChosenBook.ISBN == UrBook.ISBN)
                {
                    ChosenBookSuccess = false;
                    Console.WriteLine($"Du har redan lånat {ChosenBook.Title} av  {ChosenBook.Author}"); // Om man redan har boken går det inte att reservera 
                }
            }
            if (ChosenBookSuccess)
            {
                Console.WriteLine($"Vill du resververa {ChosenBook.Title} av {ChosenBook.Author} (y/n)");
                string ConfirmReserv = Console.ReadLine();
                if (ConfirmReserv.ToLower() == "y")
                {
                    Console.WriteLine($"Du har nu reserverat {ChosenBook.Title} av {ChosenBook.Author}");
                    return true;
                }
            }
            return false;
        }
        public static List<Book> CreateBook(List<Book> books) // funktion för bibliotikare för att skapa en bok
        {
            try
            {
                Console.WriteLine("Skriv in titel");
                string TitleInput = Console.ReadLine();
                Console.WriteLine("Skriv in författare");
                string AuthorInput = Console.ReadLine();
                Console.WriteLine("Skriv in genren");
                string SubjectInput = Console.ReadLine();
                Console.WriteLine("Skriv in ISBN nummeret som ska vara minst 13 siffor");
                string ISBNInput = Console.ReadLine();
                bool ValidISBN = true;
                foreach (Book aBook in books)
                {
                    if (ISBNInput.Length != 13 || ISBNInput == aBook.ISBN)
                    {
                        Console.WriteLine("ISBN nummeret är otillgängligt"); //Om ISBN inte är 13 nummer så är nummret otillgängligt
                        ValidISBN = false;
                    }
                    if (ValidISBN)
                    {
                        books.Add(new Book() { Title = TitleInput, Author = AuthorInput, Subject = SubjectInput, ISBN = ISBNInput, Available = true, ReservedBy = new List<User>() });
                        Console.WriteLine("Nu har boken skapats"); // lägger till en bok i listan om ISBN är "valid" 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return books;
        }
        public static List<Book> EditBook(List<Book> books) //en funktion för bibliotikarien för att kunna redigera en bok
        {
            try
            {
                Console.WriteLine("Välj en bok du vill ändra");
                Book ChosenBook = ChooseBooks(books); // den valda boken bibliotikarien har valt att ändra 
                Console.Clear();
                if(ChosenBook == null) { return books; } 
                Console.WriteLine($"1) Titel: {ChosenBook.Title}");
                Console.WriteLine($"2) Författare: {ChosenBook.Author}");
                Console.WriteLine($"3) Ämne: {ChosenBook.Subject}");
                Console.WriteLine($"4) ISBN: {ChosenBook.ISBN}");
                Console.WriteLine("5) Radera boken");
                Console.WriteLine("6) Gå tillbaka till föregående sida");

                Console.WriteLine("Skriv in nedan det du vill ändra");
                string EditInput = Console.ReadLine();
                switch (EditInput.ToLower())
                {
                    case "1":
                        Console.WriteLine("Skriv in den nya titeln");
                        string NewTitle = Console.ReadLine();
                        if (NewTitle == ChosenBook.Title)
                        {
                            Console.WriteLine("Du skrev in samma titel");
                        }
                        else
                        {
                            Console.WriteLine($"Säker att du vill byta till {NewTitle} (y/n)");
                            string ConfirmTitle = Console.ReadLine();
                            if (ConfirmTitle.ToLower() == "y")
                            {
                                Console.WriteLine("Titeln har nu ändrats");
                                ChosenBook.Title = NewTitle;
                            }
                        }
                        break;
                    case "2":
                        Console.WriteLine("Skriv in den nya författaren");
                        string NewAuthor = Console.ReadLine();
                        if (NewAuthor == ChosenBook.Author)
                        {
                            Console.WriteLine("Du skrev in samma författare");
                        }
                        else
                        {
                            Console.WriteLine($"Säker att du vill byta till {NewAuthor} (y/n)");
                            string ConfirmTitle = Console.ReadLine();
                            if (ConfirmTitle.ToLower() == "y")
                            {
                                Console.WriteLine("Författaren har nu ändrats");
                                ChosenBook.Author = NewAuthor;
                            }
                        }
                        break;
                    case "3":
                        Console.WriteLine("Skriv in det nya ämnet");
                        string NewSubject = Console.ReadLine();
                        if (NewSubject == ChosenBook.Author)
                        {
                            Console.WriteLine("Du skrev in samma ämne");
                        }
                        else
                        {
                            Console.WriteLine($"Säker på att du vill byta till {NewSubject} (y/n)");
                            string ConfirmSubject = Console.ReadLine();
                            if (ConfirmSubject.ToLower() == "y")
                            {
                                Console.WriteLine($"Det har nu ändrats till {NewSubject}");
                            }


                        }
                        break;
                    case "4":
                        Console.WriteLine("Skriv in det nya ISBN");
                        string NewISBN = Console.ReadLine();
                        bool AvailableISBN = true;
                        foreach (Book CheckBook in books)
                        {
                            if (ChosenBook.ISBN == CheckBook.ISBN)
                            {
                                Console.WriteLine("Du skrev in det förra ISBN");
                                AvailableISBN = false;
                                break;
                            }
                        }
                        if (AvailableISBN)
                        {
                            Console.WriteLine($"Säker på att du vill byta till {NewISBN} (y/n)");
                            string ConfirmISBN = Console.ReadLine();
                            if (ConfirmISBN.ToLower() == "y")
                            {
                                Console.WriteLine($"Nu har det ändrats till {NewISBN}");
                                ChosenBook.ISBN = NewISBN;
                            }
                        }
                        break;
                    case "5:":
                        Console.WriteLine($"Är du säker på att du vill tabort boken? (y/n)");
                        string ConfirmDelete = Console.ReadLine();
                        if (ConfirmDelete.ToLower() == "y")
                        {
                            books.Remove(ChosenBook);
                            Console.WriteLine("Den har nu tagits bort");
                        }
                        break;
                    case "6":
                        break;
                }
            }
            catch
            {

            }
            return books;

        }
        public static List<Book> SearchBook(List<Book> books, string input) // denna funktion är till för att kunna söka efter böcker 
        {
            List<Book> SearchedBooks = new List<Book>(); // skapar en lista för de böcker somm innefattar "input"
            foreach (Book aBook in books)
            {
                bool contains = false;
                if (aBook.Title.ToLower().Contains(input.ToLower()))
                {
                    contains = true;
                }
                else if (aBook.Author.ToLower().Contains(input.ToLower()))
                {
                    contains = true;
                }
                else if (aBook.Subject.ToLower().Contains(input.ToLower()))
                {
                    contains = true;
                }
                else if (aBook.ISBN.ToLower().Contains(input.ToLower()))
                {
                    contains = true;
                }
                if (contains)
                {
                    SearchedBooks.Add(aBook);
                }
            }
            return SearchedBooks;
        }
        public static string ViewBook(Book aBook) // skriver ut information om boken
        {
            Console.Clear(); 
            if (aBook == null)
            {
                return ""; // returnar "" tomt alltså inget ifall boken inte finns 
            }
            return $"Titel: {aBook.Title}\n" + //returnar information om boken ifall den finns 
                $"Författare: {aBook.Author}\n" +
                $"Genre: {aBook.Subject}\n" +
                $"ISBN: {aBook.ISBN}\n" +
                $"Tillgänglig: {aBook.Available}";
        }
        public static void ListAllBooks(List<Book> books) //  används för att lista alla böcker 
        {
            books.OrderBy(a => a.Author); 
            int BookNum = 1;
            foreach (var aBook in books)
            {
                Console.WriteLine($"{BookNum}. {aBook.Title} av {aBook.Author}");
                BookNum++;
            }
        }
    }
}