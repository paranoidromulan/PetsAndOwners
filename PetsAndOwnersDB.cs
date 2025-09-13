namespace PetsAndOwners;

using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

public record PetsOwners (string petname, string petspecies, string phone);

public class PetsAndOwnersDB
{
    private static string _connectionstring = "Data Source = petsandowners.db";
    public PetsAndOwnersDB()
    {

        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();

            //Table: owners id, name, phone)
            //Table: pets id, name, species, owner_id

            var createOwnersTableCmd = connection.CreateCommand();
            createOwnersTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Owners(
                    id INTEGER PRIMARY KEY,
                    name VARCHAR(225),
                    phone VARCHAR(225)
                    )";
            createOwnersTableCmd.ExecuteNonQuery();

            var createPetsTableCmd = connection.CreateCommand();
            createPetsTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Pets(
                    id INTEGER PRIMARY KEY,
                    name VARCHAR(225),
                    species VARCHAR(225),
                    owner_id INTEGER,
                    FOREIGN KEY (owner_id) REFERENCES Owners(id)
                    )";
            createPetsTableCmd.ExecuteNonQuery();

        }
    }

    public void AddOwner(string name, string phone)
    {
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var insertOwnerCommand = connection.CreateCommand();
                insertOwnerCommand.CommandText = @"
                    INSERT INTO Owners (name, phone) VALUES ($Name, $Phone)";
                insertOwnerCommand.Parameters.AddWithValue("Name", name);
                insertOwnerCommand.Parameters.AddWithValue("Phone", phone);
                insertOwnerCommand.ExecuteNonQuery();

                transaction.Commit();
            }
        }
    }
    //Method to retrieve someones ID based on their name

    public int getOwnerIdByName(string name)
    {
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            var getOwnerIdCommand = connection.CreateCommand();
            getOwnerIdCommand.CommandText = @"
            SELECT id
            FROM Owners
            WHERE name = $name";
            getOwnerIdCommand.Parameters.AddWithValue("$name", name);
            var result = getOwnerIdCommand.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }
    }

    public void AddPet(string name, string species, string ownerName)
    {
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            var getOwnerIdCommand = connection.CreateCommand();
            getOwnerIdCommand.CommandText = @"SELECT id
                                            FROM Owners
                                            WHERE name = $OwnerName";
            getOwnerIdCommand.Parameters.AddWithValue("$OwnerName", ownerName);

            var result = getOwnerIdCommand.ExecuteScalar();
            int ownerId = Convert.ToInt32(result);

            using (var transaction = connection.BeginTransaction())
            {
                var insertPetCommand = connection.CreateCommand();
                insertPetCommand.CommandText = @"
                INSERT INTO Pets (name, species, owner_id) VALUES ($Name, $Species, $OwnerId)";
                insertPetCommand.Parameters.AddWithValue("Name", name);
                insertPetCommand.Parameters.AddWithValue("Species", species);
                insertPetCommand.Parameters.AddWithValue("OwnerId", ownerId);
                insertPetCommand.ExecuteNonQuery();

                transaction.Commit();
            }
        }
    }

    //Searching for owners phone number by pet name

    public List<PetsOwners> SearchPhoneByPetName(string petName)
    {
        var results = new List<PetsOwners>();

        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            
            var searchPhoneCommand = connection.CreateCommand();
            searchPhoneCommand.CommandText = @"
            SELECT Pets.name, Pets.species, Owners.phone 
            FROM Pets 
            JOIN Owners ON Owners.id = Pets.owner_id 
            WHERE Pets.name = @petName ";
            searchPhoneCommand.Parameters.AddWithValue("@petName", petName);
            using (var reader = searchPhoneCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new PetsOwners(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                }
            }

        }

        return results;
    }

    public void ChangePhoneNumber(string ownerName, string newPhone)
    {
        var results = new List<PetsOwners>();
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();

            var changePhoneNumberCommand = connection.CreateCommand();
            changePhoneNumberCommand.CommandText = @"
            UPDATE Owners
            SET phone = @newPhone
            WHERE name = @ownerName";
            changePhoneNumberCommand.Parameters.AddWithValue("@newPhone", newPhone);
            changePhoneNumberCommand.Parameters.AddWithValue("@ownerName", ownerName);
            changePhoneNumberCommand.ExecuteScalar();
        }

    }

    public List<PetsOwners> GetPetsOwners()
    {
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            var selectOwnersCmd = connection.CreateCommand();
            selectOwnersCmd.CommandText = @"
            SELECT Pets.name, Pets.species, Owners.phone
            FROM Pets, Owners";
            using (var reader = selectOwnersCmd.ExecuteReader())
            {
                List<PetsOwners> petsowners = new List<PetsOwners>();
                while (reader.Read())
                {
                    petsowners.Add(new PetsOwners(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                }
                return petsowners;
            }
        }
    }
    
}