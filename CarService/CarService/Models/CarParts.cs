namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class CarParts
    {
        public int Id { get; set; }
        public string PartDescr { get; set; }
        public int CarBrandId { get; set; }
        public decimal Price { get; set; }

        internal AppDb Db { get; set; }

        public CarParts()
        {
        }

        internal CarParts(AppDb db)
        {
            Db = db;
        }

        public async Task<CarParts> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `CarParts` (`PartDescr`, `CarBrandId`, `Price`) VALUES (@partDescr, @carBrandId, @price);";
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
                ParameterName = "@partDescr",
                DbType = DbType.String,
                Value = PartDescr,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carBrandId",
                DbType = DbType.Int32,
                Value = CarBrandId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@price",
                DbType = DbType.Decimal,
                Value = Price,
            });
        }
    }
}