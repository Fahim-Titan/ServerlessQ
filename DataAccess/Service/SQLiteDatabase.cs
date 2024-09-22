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
                            LastName TEXT NOT NULL
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

        public async Task<Person> SaveData(Person person)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Person (FirstName, LastName)
                        VALUES (@FirstName, @LastName);
                        SELECT last_insert_rowid();
                    ";
                    command.Parameters.AddWithValue("@FirstName", person.FirstName);
                    command.Parameters.AddWithValue("@LastName", person.LastName);
                    var result = await command.ExecuteScalarAsync();
                    person.Id = Convert.ToInt32(result);
                    return person;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured in Saving data in SQL DB", ex);
            }
        }

    }
}
