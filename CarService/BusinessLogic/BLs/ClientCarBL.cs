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
            cmd.CommandText = @"INSERT INTO `ClientCars` (`ClientId`, `CarBrandId`, `LicensePlate`, `Mileage`, `Archived`) VALUES (@clientId, @carBrandId, @licensePlate, @mileage, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<ClientCarDTO>> GetAllAsync()
        {
            var results = new List<ClientCarDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `ClientCars`.`Id`,
       `ClientCars`.`ClientId`,
       `Clients`.`FirstName` AS `ClientFirstName`,
       `Clients`.`LastName` AS `ClientLastName`,
       `ClientCars`.`CarBrandId`,
       `CarBrands`.`BrandName` AS `CarBrandName`,
       `ClientCars`.`LicensePlate`,
       `ClientCars`.`Mileage`,
       `ClientCars`.`Archived`
FROM `ClientCars`
INNER JOIN `Clients` ON `Clients`.`Id` = `ClientCars`.`ClientId`
INNER JOIN `CarBrands` ON `CarBrands`.`Id` = `ClientCars`.`CarBrandId`
ORDER BY `ClientCars`.`Id`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ClientCarDTO
                {
                    Id = reader.GetInt32("Id"),
                    ClientId = reader.GetInt32("ClientId"),
                    ClientFirstName = reader.GetString("ClientFirstName"),
                    ClientLastName = reader.GetString("ClientLastName"),
                    CarBrandId = reader.GetInt32("CarBrandId"),
                    CarBrandName = reader.GetString("CarBrandName"),
                    LicensePlate = reader.GetString("LicensePlate"),
                    Mileage = reader.GetInt32("Mileage"),
                    Archived = reader.GetBoolean("Archived"),
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
                ParameterName = "@licensePlate",
                DbType = DbType.String,
                Value = dto.LicensePlate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@mileage",
                DbType = DbType.Int32,
                Value = dto.Mileage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@archived",
                DbType = DbType.Boolean,
                Value = dto.Archived,
            });
        }
    }
}
