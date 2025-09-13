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

    public void AddPet(string name, string species)
    {
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var insertPetCommand = connection.CreateCommand();
                insertPetCommand.CommandText = @"
                INSERT INTO Pets (name, species) VALUES ($Name, $Species)";
                insertPetCommand.Parameters.AddWithValue("Name", name);
                insertPetCommand.Parameters.AddWithValue("Species", species);
                insertPetCommand.ExecuteNonQuery();

                transaction.Commit();
            }
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