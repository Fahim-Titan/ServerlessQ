using DataAccess.Interface;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    internal class SQLiteDatabase: IDataAccess
    {
        private readonly string _databasePath;
        public SQLiteDatabase()
        {
            _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        }

        public Task SaveData(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
