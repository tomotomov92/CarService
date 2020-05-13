namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class CarServices
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }

        internal AppDb Db { get; set; }

        public CarServices()
        {
        }

        internal CarServices(AppDb db)
        {
            Db = db;
        }

        public async Task<CarServices> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `CarServices` (`ServiceName`) VALUES (@serviceName);";
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
                ParameterName = "@serviceName",
                DbType = DbType.String,
                Value = ServiceName,
            });
        }
    }
}