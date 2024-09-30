using DataAccess.Interface;
using DataAccess.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class SQLiteDatabase: IRepository
    {
        private readonly string _databasePath;
        public SQLiteDatabase()
        {
            _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Person (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            FirstName TEXT NOT NULL,
                            LastName TEXT NOT NULL,
                            Svg TEXT NULL
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured during the table creation", ex);
            }
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection($"Data Source={_databasePath}");
        }

        private async Task<int> InsertPersonAsync(SqliteConnection connection, Person person)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Person (FirstName, LastName)
                VALUES (@FirstName, @LastName);
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("@FirstName", person.FirstName);
            command.Parameters.AddWithValue("@LastName", person.LastName);
            
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Person> SaveData(Person person)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    var personId = await InsertPersonAsync(connection, person);
                    person.Id = personId;
                    person.Svg = string.Empty;
                    return person;
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception instead of wrapping and re-throwing it
                throw;
            }
        }

        private async Task<Person> GetPersonByIdAsync(SqliteConnection connection, int id)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, FirstName, LastName, Svg
                FROM Person
                WHERE Id = @Id;
            ";
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return MapReaderToPerson(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        private Person MapReaderToPerson(SqliteDataReader reader)
        {
            return new Person
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Svg = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
            };
        }

        public async Task<Person?> GetPersonByFirstAndLastName(string firstName, string lastName)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT Id, FirstName, LastName, Svg
                        FROM Person
                        WHERE FirstName = @first AND LastName = @last;
                    ";
                    command.Parameters.AddWithValue("@first", firstName);
                    command.Parameters.AddWithValue("@last", lastName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var person = new Person
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Svg = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                            };
                            return person;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred in retrieving data from SQL DB", ex);
            }
        }

        public async Task<Person> GetPerson(int id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    return await GetPersonByIdAsync(connection, id);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception instead of wrapping and re-throwing it
                throw;
            }
        }

        public async Task<Person> SaveSvgData(Person person, string svg)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        UPDATE Person
                        SET Svg = @Svg
                        WHERE Id = @Id;
                    ";
                    command.Parameters.AddWithValue("@Svg", svg);
                    command.Parameters.AddWithValue("@Id", person.Id);
                    await command.ExecuteNonQueryAsync();
                    return await GetPerson(person.Id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred in updating data in SQL DB", ex);
            }
        }
    }
}
