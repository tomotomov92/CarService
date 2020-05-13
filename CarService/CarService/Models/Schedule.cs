namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class Schedule
    {
        public int Id { get; set; }
        public DateTime ForDate { get; set; }
        public decimal HourBegin { get; set; }
        public decimal HourEnd { get; set; }
        public int EmployeeId { get; set; }

        internal AppDb Db { get; set; }

        public Schedule()
        {
        }

        internal Schedule(AppDb db)
        {
            Db = db;
        }

        public async Task<Schedule> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Schedule` (`ForDate`, `HourBegin`, `HourEnd`, `EmployeeId`) VALUES (@forDate, @hourBegin, @hourEnd, @employeeId);";
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
                ParameterName = "@forDate",
                DbType = DbType.DateTime,
                Value = ForDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hourBegin",
                DbType = DbType.Decimal,
                Value = HourBegin,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hourEnd",
                DbType = DbType.Decimal,
                Value = HourEnd,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@employeeId",
                DbType = DbType.Int32,
                Value = EmployeeId,
            });
        }
    }
}