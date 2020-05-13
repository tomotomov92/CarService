namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfStart { get; set; }

        internal AppDb Db { get; set; }

        public Employee()
        {
        }

        internal Employee(AppDb db)
        {
            Db = db;
        }

        public async Task<Employee> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Employee` (`FirstName`, `LastName`, `DateOfStart`) VALUES (@firstName, @lastName, @dateOfStart);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
            return this;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateOfStart",
                DbType = DbType.DateTime,
                Value = DateOfStart,
            });
        }
    }
}