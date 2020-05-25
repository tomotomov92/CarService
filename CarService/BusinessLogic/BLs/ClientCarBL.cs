using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class ClientCarBL : BaseBL<ClientCarDTO>
    {
        public ClientCarBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<ClientCarDTO> AddAsync(ClientCarDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ClientCar` (`ClientId`, `CarBrandId`, `Mileage`) VALUES (@clientId, @carBrandId, @mileage);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<ClientCarDTO>> GetAllAsync()
        {
            var results = new List<ClientCarDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, ``ClientId`, `CarBrandId`, `Mileage` FROM `ClientCar`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ClientCarDTO
                {
                    Id = reader.GetInt32("Id"),
                    ClientId = reader.GetInt32("ClientId"),
                    CarBrandId = reader.GetInt32("CarBrandId"),
                    Mileage = reader.GetInt32("Mileage"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, ClientCarDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientId",
                DbType = DbType.Int32,
                Value = dto.ClientId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carBrandId",
                DbType = DbType.Int32,
                Value = dto.CarBrandId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@mileage",
                DbType = DbType.Int32,
                Value = dto.Mileage,
            });
        }
    }
}
