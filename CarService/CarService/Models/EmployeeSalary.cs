namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class EmployeeSalary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Salary { get; set; }

        internal AppDb Db { get; set; }

        public EmployeeSalary()
        {
        }

        internal EmployeeSalary(AppDb db)
        {
            Db = db;
        }

        public async Task<EmployeeSalary> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `EmployeeSalary` (`EmployeeId`, `Salary`) VALUES (@employeeId, @salary);";
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
                ParameterName = "@employeeId",
                DbType = DbType.Int32,
                Value = EmployeeId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@salary",
                DbType = DbType.Decimal,
                Value = Salary,
            });
        }
    }
}