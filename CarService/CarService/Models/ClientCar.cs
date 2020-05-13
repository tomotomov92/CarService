namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Threading.Tasks;

    public class ClientCar
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarBrandId { get; set; }
        public int Mileage { get; set; }

        internal AppDb Db { get; set; }

        public ClientCar()
        {
        }

        internal ClientCar(AppDb db)
        {
            Db = db;
        }

        public async Task<ClientCar> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ClientCar` (`ClientId`, `CarBrandId`, `Mileage`) VALUES (@clientId, @carBrandId, @mileage);";
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
                ParameterName = "@clientId",
                DbType = DbType.Int32,
                Value = ClientId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carBrandId",
                DbType = DbType.Int32,
                Value = CarBrandId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@mileage",
                DbType = DbType.Int32,
                Value = Mileage,
            });
        }
    }
}