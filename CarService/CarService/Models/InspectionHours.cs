namespace CarService.Models
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class InspectionHours
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public DateTime DateTimeOfInspection { get; set; }

        internal AppDb Db { get; set; }

        public InspectionHours()
        {
        }

        internal InspectionHours(AppDb db)
        {
            Db = db;
        }

        public async Task<InspectionHours> AddAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `InspectionHours` (`ClientId`, `CarId`, `DateTimeOfInspection`) VALUES (@clientId, @carId, @dateTimeOfInspection);";
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
                ParameterName = "@carId",
                DbType = DbType.Int32,
                Value = CarId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateTimeOfInspection",
                DbType = DbType.DateTime,
                Value = DateTimeOfInspection,
            });
        }
    }
}