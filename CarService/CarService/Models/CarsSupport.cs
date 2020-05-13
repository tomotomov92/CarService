namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class CarsSupport
    {
        public int Id { get; set; }
        public string Support { get; set; }
        public int CarBrandId { get; set; }

        internal AppDb Db { get; set; }

        public CarsSupport()
        {
        }

        internal CarsSupport(AppDb db)
        {
            Db = db;
        }

        public async Task<CarsSupport> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `CarsSupport` (`Support`, `CarBrandId`) VALUES (@support, @carBrandId);";
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
                ParameterName = "@support",
                DbType = DbType.String,
                Value = Support,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carBrandId",
                DbType = DbType.Int32,
                Value = CarBrandId,
            });
        }
    }
}