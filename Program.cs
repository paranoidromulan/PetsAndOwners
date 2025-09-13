namespace PetsAndOwners;

using Microsoft.Data.Sqlite;

class Program
{


    static void Main(string[] args)
    {
        PetsAndOwnersDB petsAndOwnersDB = new PetsAndOwnersDB();

        Console.WriteLine("Welcome to the pets and owners service");
        Console.WriteLine("Please select the number of the action you would like to do:\n1 - Add an owner\n2 - Add a pet\n3 - See all pets and owners\n4 - Upadte an owner's phone number\n5 - Search an owner's phone number based on the pet's name\n6 - End the program");

        string? input = Console.ReadLine();

        if (input == "1")
        {
            Console.WriteLine("Please provide the name of the owner:");
            string? name = Console.ReadLine();

            Console.WriteLine("Now provide the phone number of the owner:");
            string? phone = Console.ReadLine();

            petsAndOwnersDB.AddOwner(name, phone);
        }
        if (input == "2")
        {
            Console.WriteLine("Please provide the name of the pet:");
            string? name = Console.ReadLine();

            Console.WriteLine("Now provide the species of the pet:");
            string? species = Console.ReadLine();

            petsAndOwnersDB.AddPet(name, species);
        }

        if (input == "3")
        {
            List<PetsOwners> allpets = petsAndOwnersDB.GetPetsOwners();
            foreach (PetsOwners petsowners in allpets)
            {
                Console.WriteLine($"{petsowners.petname}, {petsowners.petspecies}, {petsowners.phone}");
            }
        }

        if (input == "5")
            {
                return;
            }


    }
}
