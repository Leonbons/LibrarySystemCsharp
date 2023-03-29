using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace LibrarySystem
{
    class User
    {
        // En klass som skapar strängar för medlem
        public string PersonalNum { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool CurrentUser { get; set; }
        public bool Librarian { get; set; }

        public List<Book> MyBooks { get; set; }
        public override string ToString() // Skriver ut olika värden för klassen
        {
            return $"{PersonalNum},{FirstName},{LastName},{Password},{CurrentUser},{Librarian}";
        }

        public static List<User> Logout(List<User> users, User aUser)
        {
            aUser.CurrentUser = false; 
            Console.WriteLine("Det gick att logga ut!");
            return users;
        }
        public static List<User> ManageAccount(List<User> users, User aUser)
        {
            Console.WriteLine($"Ditt förnamn: {aUser.FirstName} ");
            Console.WriteLine($"Ditt efternamn: {aUser.LastName}");
            Console.WriteLine($"Ditt lösenord: {aUser.Password}");
            Console.WriteLine($"Ditt personnummer: {aUser.PersonalNum}");

            Console.WriteLine("Välj nedan ett alternativ du vill göra");
            Console.WriteLine("1) Gå till föregående sida");
            Console.WriteLine("2) Ändra lösenord");
            Console.WriteLine("3) Logga  ut");
            string ManageInput = Console.ReadLine();
            switch (ManageInput) //switch-sats för att ge användaren alternativ 
            {
                case "1":
                    break;
                case "2":
                    Console.WriteLine("Skriv in ditt nya lösenord");
                    string NewPassword = Console.ReadLine();
                    if (NewPassword == aUser.Password) // Om det nya lösenorden är samma som det gamla 
                    {
                        Console.WriteLine("Du skrev in samma lösenord");
                    }
                    else
                    {
                        aUser.Password = NewPassword; // det nya lösenordets sätts lagras i stringen "Password"
                        Console.WriteLine($"Ditt nya lösenord är: {NewPassword}");
                    }
                    break;
                case "3":
                    Logout(users, aUser);
                    break;

            }
            return users;
        }
        public static List<User> Login(List<User> users, List<Book> books)
        {
            Console.WriteLine("Skriv ditt personnummer");
            string PersonalNumInput = Console.ReadLine();
            Console.WriteLine("Skriv in ditt lösenord");
            string PasswordInput = Console.ReadLine();

            bool SigninSuccess = false;

            foreach (User aUser in users)
            {
                if (PersonalNumInput == aUser.PersonalNum && PasswordInput == aUser.Password) //Om Personnummret och lösenordet är samma som kontots, loggas man in
                {
                    Console.WriteLine($"Hej {aUser.FirstName}");
                    aUser.CurrentUser = true;
                    SigninSuccess = true; //SigninSuccess blir true ifall det går att logga in
                }
            }
            if (SigninSuccess == false)
            {
                Console.WriteLine("Det gick inte att logga in, kontrollera så du skriver rätt");
            }
            return users;
        }
        public static List<User> RegistrationPage(List<User> users)
        {
            Console.WriteLine("Hej, välkommen till registerationen");

            Console.WriteLine("Skriv in ditt förnamn");
            string FirstNameInput = Console.ReadLine();

            Console.WriteLine("Skriv in ditt efternamn");
            string LastNameInput = Console.ReadLine();

            Console.WriteLine("Skriv in ditt personnummer");
            string PersonalNumInput = Console.ReadLine();

            Console.WriteLine("Skriv in ditt nya lösenord");
            string PasswordInput = Console.ReadLine();

            if (PersonalNumInput.Length == 12)
            {
                users.Add(new User() { FirstName = FirstNameInput, LastName = LastNameInput, PersonalNum = PersonalNumInput, Password = PasswordInput, CurrentUser = false });
                Console.WriteLine("Det är godkänt, ditt konto har nu skapats");
            }
            else
            {
                Console.WriteLine("Ditt personnummer är ogiltligt");
            }
            return users;
        }
        public static List<User> EditUser(List<User> users)
        {
            Console.WriteLine("Användare: ");
            var sort = from aUser in users // Sorterar användarna efter efternamnen
                       orderby aUser.LastName
                       select aUser;
            int UserNum1 = 1;
            foreach (var aUser in sort) // foreach-sats som skriver ut alla användare
            {
                Console.WriteLine($"{UserNum1}, {aUser.FirstName},{aUser.LastName}");
                UserNum1++;
            }
            try
            {
                Console.WriteLine("Välj en användare nedan");
                int ChosenUser = int.Parse(Console.ReadLine());
                int UserNum2 = 1;
                bool ChosenUserSuccess = false;
                foreach (var aUser in sort)
                {
                    if (ChosenUser == UserNum2) // Går det att välja en medlem skriver den ut dess information
                    {
                        ChosenUserSuccess = true;
                        Console.Clear();
                        Console.WriteLine("Skriv in något du vill ändra");
                        Console.WriteLine($" 1) Förnamn: {aUser.FirstName}");
                        Console.WriteLine($" 2) LastName: {aUser.LastName}");
                        Console.WriteLine($" 3) Lösenord: {aUser.Password}");
                        Console.WriteLine($" 4) Personnummer: {aUser.PersonalNum}");
                        Console.WriteLine($" 5) Radera användaren");
                        Console.WriteLine($" 6) Gå tillbaka");
                        Console.WriteLine($"Vad vill du göra?");
                        string EditUserInput = Console.ReadLine();

                        switch (EditUserInput.ToLower())
                        {
                            case "1":
                                Console.WriteLine($"Skriv in det nya förnamnet:");
                                string NewFirstName = Console.ReadLine();
                                if (NewFirstName == aUser.FirstName)
                                {
                                    Console.WriteLine($"Går ej att byta till samma namn");
                                }
                                else
                                {
                                    Console.WriteLine($"Vill du byta namnet till {NewFirstName} (y/n)");
                                    string ConfirmNewFirstName = Console.ReadLine();
                                    if (ConfirmNewFirstName == "y")
                                    {
                                        aUser.FirstName = NewFirstName;
                                    }
                                }
                                break;
                            case "2":
                                Console.WriteLine($"Skriv in det nya efternamnet:");
                                string NewLastName = Console.ReadLine();
                                if (NewLastName == aUser.LastName)
                                {
                                    Console.WriteLine($"Går ej att byta till samma efternamn");
                                }
                                else
                                {
                                    Console.WriteLine($"Vill du byta efternamnet till {NewLastName} (y/n)");
                                    string ConfirmNewLastName = Console.ReadLine();
                                    if (ConfirmNewLastName == "y")
                                    {
                                        aUser.LastName = NewLastName;
                                    }
                                }
                                break;
                            case "3":
                                Console.WriteLine($"Skriv in det nya Lösenordet:");
                                string NewPassword = Console.ReadLine();
                                if (NewPassword == aUser.Password)
                                {
                                    Console.WriteLine($"Går ej att byta till samma lösenord");
                                }
                                else
                                {
                                    Console.WriteLine($"Vill du byta lösenordet till {NewPassword} (y/n)");
                                    string ConfirmNewPassword = Console.ReadLine();
                                    if (ConfirmNewPassword == "y")
                                    {
                                        aUser.Password = NewPassword;
                                    }
                                }
                                break;
                            case "4":
                                Console.WriteLine($"Skriv in det nya personnummret:");
                                string NewPersonalNum = Console.ReadLine();
                                bool ValidPersonalNum = true;
                                foreach (User CheckUser in users)
                                {
                                    if (NewPersonalNum == CheckUser.PersonalNum)
                                    {
                                        Console.WriteLine($"Går ej att byta till samma personnummer");
                                        ValidPersonalNum = false;
                                        break;
                                    }
                                }
                                if (ValidPersonalNum)
                                {
                                    Console.WriteLine($"Vill du byta namnet till {NewPersonalNum} (y/n)");
                                    string ConfirmNewPersonalNum = Console.ReadLine();
                                    if (ConfirmNewPersonalNum == "y")
                                    {
                                        aUser.PersonalNum = NewPersonalNum;
                                    }
                                }
                                break;
                            case "5":
                                Console.WriteLine($"Säker på att du vill tabort {aUser.FirstName} {aUser.LastName} från biblioteket? (y/n)");
                                string ConfirmUserRemoval = Console.ReadLine();
                                if (ConfirmUserRemoval.ToLower() == "y")
                                {
                                    Console.WriteLine($"Nu är {aUser.FirstName} {aUser.LastName} borttagen");
                                    users.Remove(aUser);
                                }
                                break;
                            case "6":
                                break;
                        }
                        if (ChosenUserSuccess == false)
                        {
                            Console.WriteLine("Användaren du letar efter finns inte");
                        }
                    }
                    UserNum2++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return users;
        }

    }
}