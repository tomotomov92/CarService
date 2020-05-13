namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class PriceList
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public decimal Price { get; set; }

        internal AppDb Db { get; set; }

        public PriceList()
        {
        }

        internal PriceList(AppDb db)
        {
            Db = db;
        }

        public async Task<PriceList> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `PriceList` (`Service`, `Price`) VALUES (@service, @price);";
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
                ParameterName = "@service",
                DbType = DbType.String,
                Value = Service,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@price",
                DbType = DbType.String,
                Value = Price,
            });
        }
    }
}