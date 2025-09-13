namespace PetsAndOwners;

using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

public record PetsOwners ();

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
                    owner_id INTEGER,
                    FOREIGN KEY (owner_id) REFERENCES Owners(id)
                    )";
            createPetsTableCmd.ExecuteNonQuery();     
                        
        }
    }
    
}