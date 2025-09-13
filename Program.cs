namespace PetsAndOwners;

using Microsoft.Data.Sqlite;

class Program
{


    static void Main(string[] args)
    {
        PetsAndOwnersDB petsAndOwnersDB = new PetsAndOwnersDB();

        Console.WriteLine("Welcome to the pets and owners service");
        Console.WriteLine("Please select the number of the action you would like to do:\n1 - Add a pet, an owner\n2 - Upadte an owner's phone number\n3 - Search an owner's phone number based on the pet's name\n4 - End the program");

        string? input = Console.ReadLine();

        if (input == "4")
        {
            return;
        }


    }
}
