using System;
using System.Collections.Generic; // For List<T>
using System.Linq;                // For LINQ methods like Where, OrderBy, First, ToList

// Class to represent a single contact in the phonebook
public class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public Contact(string firstName, string lastName, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    // Override ToString for easy printing in the specified format
    public override string ToString()
    {
        return $"Name: {FirstName} Surname: {LastName} Phone Number: {PhoneNumber}";
    }
}

// Class to manage the phonebook operations
public class Phonebook
{
    private List<Contact> _contacts;

    public Phonebook()
    {
        _contacts = new List<Contact>();
        AddDefaultContacts();
    }

    // Adds 5 default contacts to the phonebook
    private void AddDefaultContacts()
    {
        _contacts.Add(new Contact("Ali", "Yilmaz", "5551112233"));
        _contacts.Add(new Contact("Ayse", "Demir", "5554445566"));
        _contacts.Add(new Contact("Can", "Kaya", "5557778899"));
        _contacts.Add(new Contact("Derya", "Celikkol", "5551234567"));
        _contacts.Add(new Contact("Emre", "Kurt", "5559876543"));
    }

    // Displays the main menu to the user
    public void DisplayMenu()
    {
        Console.WriteLine("\nPlease select an operation you want to perform :)");
        Console.WriteLine("*******************************************");
        Console.WriteLine("(1) Save New Number");
        Console.WriteLine("(2) Delete Existing Number");
        Console.WriteLine("(3) Update Existing Number");
        Console.WriteLine("(4) List Phonebook");
        Console.WriteLine("(5) Search Phonebook");
        Console.WriteLine("(0) Exit Application"); // Added exit option
        Console.Write("Your choice: ");
    }

    // (1) Saves a new number to the phonebook
    public void SaveNewNumber()
    {
        Console.Write("Please enter name             : ");
        string firstName = Console.ReadLine();
        Console.Write("Please enter last name          : ");
        string lastName = Console.ReadLine();
        Console.Write("Please enter phone number : ");
        string phoneNumber = Console.ReadLine();

        // Basic validation for non-empty fields
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.WriteLine("Name, last name, and phone number cannot be empty. Please try again.");
            return;
        }

        _contacts.Add(new Contact(firstName, lastName, phoneNumber));
        Console.WriteLine("Contact saved successfully!");
    }

    // (2) Deletes an existing number from the phonebook
    public void DeleteExistingNumber()
    {
        Console.Write("Please enter the name or last name of the person you want to delete: ");
        string searchKeyword = Console.ReadLine();

        List<Contact> matchingContacts = _contacts
            .Where(c => c.FirstName.Equals(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                        c.LastName.Equals(searchKeyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingContacts.Count == 0)
        {
            Console.WriteLine("No data matching your criteria was found in the phonebook. Please make a selection.");
            HandleNotFoundOption(DeleteExistingNumber, "delete"); // Pass the method itself for retry
            return;
        }
        
        Contact contactToDelete = matchingContacts.First(); // As per requirement, delete the first found

        Console.Write($"{contactToDelete.FirstName} {contactToDelete.LastName} is about to be deleted from the phonebook, do you approve? (y/n): ");
        string confirmation = Console.ReadLine();

        if (confirmation != null && confirmation.ToLower() == "y")
        {
            _contacts.Remove(contactToDelete);
            Console.WriteLine("Contact deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    // (3) Updates an existing number in the phonebook
    public void UpdateExistingNumber()
    {
        Console.Write("Please enter the name or last name of the person whose number you want to update: ");
        string searchKeyword = Console.ReadLine();

        List<Contact> matchingContacts = _contacts
            .Where(c => c.FirstName.Equals(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                        c.LastName.Equals(searchKeyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingContacts.Count == 0)
        {
            Console.WriteLine("No data matching your criteria was found in the phonebook. Please make a selection.");
            HandleNotFoundOption(UpdateExistingNumber, "update"); // Pass the method itself for retry
            return;
        }
        
        Contact contactToUpdate = matchingContacts.First(); // As per requirement, update the first found

        Console.WriteLine($"Updating contact: {contactToUpdate.FirstName} {contactToUpdate.LastName}");
        Console.Write("Enter new name (leave blank to keep current): ");
        string newFirstName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newFirstName))
        {
            contactToUpdate.FirstName = newFirstName;
        }

        Console.Write("Enter new last name (leave blank to keep current): ");
        string newLastName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newLastName))
        {
            contactToUpdate.LastName = newLastName;
        }

        Console.Write("Enter new phone number (leave blank to keep current): ");
        string newPhoneNumber = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPhoneNumber))
        {
            contactToUpdate.PhoneNumber = newPhoneNumber;
        }

        Console.WriteLine("Contact updated successfully!");
    }

    // Helper method for handling "contact not found" scenarios for delete/update
    private void HandleNotFoundOption(Action retryAction, string operationType)
    {
        Console.WriteLine($"* To end the {operationType}: (1)");
        Console.WriteLine("* To try again        : (2)");
        Console.Write("Your choice: ");
        string choice = Console.ReadLine();

        if (choice == "2")
        {
            retryAction.Invoke(); // Call the original method again
        }
        else if (choice != "1")
        {
            Console.WriteLine("Invalid choice. Returning to main menu.");
        }
    }

    // (4) Lists all contacts in the phonebook
    public void ListPhonebook()
    {
        Console.WriteLine("\nPhonebook");
        Console.WriteLine("**********************************************");

        if (_contacts.Count == 0)
        {
            Console.WriteLine("Phonebook is empty.");
            return;
        }

        Console.WriteLine("Please select sorting order:");
        Console.WriteLine("(1) Sort A-Z (by Name)");
        Console.WriteLine("(2) Sort Z-A (by Name)");
        Console.Write("Your choice: ");
        string sortChoice = Console.ReadLine();

        IEnumerable<Contact> sortedContacts;

        if (sortChoice == "1")
        {
            sortedContacts = _contacts.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
        }
        else if (sortChoice == "2")
        {
            sortedContacts = _contacts.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
        }
        else
        {
            Console.WriteLine("Invalid sort choice. Listing without specific sorting.");
            sortedContacts = _contacts; // Default to current order
        }

        foreach (var contact in sortedContacts)
        {
            Console.WriteLine(contact);
        }
    }

    // (5) Searches the phonebook
    public void SearchPhonebook()
    {
        Console.WriteLine("\nSelect search type:");
        Console.WriteLine("**********************************************");
        Console.WriteLine("To search by name or last name: (1)");
        Console.WriteLine("To search by phone number     : (2)");
        Console.Write("Your choice: ");
        string searchTypeChoice = Console.ReadLine();

        List<Contact> searchResults = new List<Contact>();

        if (searchTypeChoice == "1")
        {
            Console.Write("Enter name or last name to search: ");
            string keyword = Console.ReadLine();
            searchResults = _contacts
                .Where(c => c.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            c.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        else if (searchTypeChoice == "2")
        {
            Console.Write("Enter phone number to search: ");
            string keyword = Console.ReadLine();
            searchResults = _contacts
                .Where(c => c.PhoneNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        else
        {
            Console.WriteLine("Invalid search type choice. Returning to main menu.");
            return;
        }

        Console.WriteLine("\nSearch Results:");
        Console.WriteLine("**********************************************");

        if (searchResults.Count == 0)
        {
            Console.WriteLine("No matching contacts found.");
        }
        else
        {
            foreach (var contact in searchResults)
            {
                Console.WriteLine(contact);
            }
        }
    }
}

// Main program class
public class Program
{
    public static void Main(string[] args)
    {
        Phonebook phonebook = new Phonebook();
        bool continueApp = true;

        while (continueApp)
        {
            phonebook.DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    phonebook.SaveNewNumber();
                    break;
                case "2":
                    phonebook.DeleteExistingNumber();
                    break;
                case "3":
                    phonebook.UpdateExistingNumber();
                    break;
                case "4":
                    phonebook.ListPhonebook();
                    break;
                case "5":
                    phonebook.SearchPhonebook();
                    break;
                case "0": // Exit the application
                    continueApp = false;
                    Console.WriteLine("Exiting Phonebook Application. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
            // Only prompt to return to menu if the app is not exiting
            if (continueApp) {
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
            }
        }
    }
}