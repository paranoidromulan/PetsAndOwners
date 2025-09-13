namespace PetsAndOwners;

using System.Data.Common;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Data.Sqlite;

class Program
{


    static void Main(string[] args)
    {
        PetsAndOwnersDB petsAndOwnersDB = new PetsAndOwnersDB();

        
        while (true)
        {
            Console.WriteLine("Welcome to the pets and owners service");
            Console.WriteLine("Please select what you would like to do:\n1 - Add an owner\n2 - Add a pet\n3 - See all pets and owners\n4 - Update an owner's phone number\n5 - Search an owner's phone number based on the pet's name\n6 - End the program");

            string? input = Console.ReadLine();
            if (input == "1")
            {
                Console.WriteLine("Please provide the name of the owner:");
                string? name = Console.ReadLine();

                Console.WriteLine("Now provide the phone number of the owner:");
                string? phone = Console.ReadLine();

                petsAndOwnersDB.AddOwner(name, phone);

                Console.WriteLine("The owner has been added to the database!");

            }
            if (input == "2")
            {
                Console.WriteLine("Please provide the name of the pet:");
                string? name = Console.ReadLine();

                Console.WriteLine("Now provide the species of the pet:");
                string? species = Console.ReadLine();

                Console.WriteLine("Next, provide the name of the pet's owner (it must already exist)");
                string? ownerName = Console.ReadLine();


                petsAndOwnersDB.AddPet(name, species, ownerName);

                Console.WriteLine("The pet has been added to the database!");
            }

            if (input == "3")
            {
                Console.WriteLine("Here are all the pets listed on the database with their owners phone numbers:");

                List<PetsOwners> allpets = petsAndOwnersDB.GetPetsOwners();
                foreach (PetsOwners petsowners in allpets)
                {
                    Console.WriteLine($"{petsowners.petname}, {petsowners.petspecies}, {petsowners.phone}");
                }
            }
            if (input == "4")
            {
                Console.WriteLine("Please provide the name of the owner whose phone number you would like to change");
                string? ownerName = Console.ReadLine();
                Console.WriteLine("Next, provide the new phone number");
                string? newPhone = Console.ReadLine();

                petsAndOwnersDB.ChangePhoneNumber(ownerName, newPhone);

                Console.WriteLine("The phone number has been updated");
            }
            if (input == "5")
            {
                Console.WriteLine("Please provide the name of the pet whose owner's phone you would like to find");
                string? petName = Console.ReadLine();

                List<PetsOwners> allpets = petsAndOwnersDB.SearchPhoneByPetName(petName);

                foreach (PetsOwners petsowners in allpets)
                {
                    Console.WriteLine($"Phone number: {petsowners.phone}");
                }
            }
            if (input == "6")
            {
                break;
            }
        }



    }
}
