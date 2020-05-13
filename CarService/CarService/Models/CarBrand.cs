namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class CarBrand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }

        internal AppDb Db { get; set; }

        public CarBrand()
        {
        }

        internal CarBrand(AppDb db)
        {
            Db = db;
        }

        public async Task<CarBrand> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `CarBrand` (`BrandName`) VALUES (@brandName);";
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
                ParameterName = "@brandName",
                DbType = DbType.String,
                Value = BrandName,
            });
        }
    }
}