using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class InspectionHoursBL : BaseBL<InspectionHoursDTO>
    {
        public InspectionHoursBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<InspectionHoursDTO> AddAsync(InspectionHoursDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `InspectionHours` (`ClientId`, `CarId`, `DateTimeOfInspection`) VALUES (@clientId, @carId, @dateTimeOfInspection);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<InspectionHoursDTO>> GetAllAsync()
        {
            var results = new List<InspectionHoursDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `InspectionHours`.`Id`,
       `InspectionHours`.`ClientId`,
       `Client`.`FirstName` AS `ClientFirstName`,
       `Client`.`LastName` AS `ClientLastName`,
       `InspectionHours`.`CarId`,
       `InspectionHours`.`DateTimeOfInspection`,
       `CarBrand`.`BrandName` AS `CarBrandName`
FROM `InspectionHours`
INNER JOIN `Client` ON `Client`.`Id` = `InspectionHours`.`ClientId`
INNER JOIN `ClientCar` ON `ClientCar`.`Id` = `InspectionHours`.`CarId`
INNER JOIN `CarBrand` ON `CarBrand`.`Id` = `ClientCar`.`CarBrandId`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new InspectionHoursDTO
                {
                    Id = reader.GetInt32("Id"),
                    ClientId = reader.GetInt32("ClientId"),
                    ClientFirstName = reader.GetString("ClientFirstName"),
                    ClientLastName = reader.GetString("ClientLastName"),
                    CarId = reader.GetInt32("CarId"),
                    CarBrandName = reader.GetString("CarBrandName"),
                    DateTimeOfInspection = reader.GetDateTime("DateTimeOfInspection"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, InspectionHoursDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientId",
                DbType = DbType.String,
                Value = dto.ClientId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carId",
                DbType = DbType.String,
                Value = dto.CarId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateTimeOfInspection",
                DbType = DbType.DateTime,
                Value = dto.DateTimeOfInspection,
            });
        }
    }
}
