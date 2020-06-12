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
            cmd.CommandText = @"INSERT INTO `InspectionHours` (`ClientId`, `CarId`, `Mileage`, `DateTimeOfInspection`, `Description`, `Archived`) VALUES (@clientId, @carId, @mileage, @dateTimeOfInspection, @description, @archived);";
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
       `InspectionHours`.`Mileage`,
       `InspectionHours`.`DateTimeOfInspection`,
       `ClientCar`.`LicensePlate` AS `CarLicensePlate`,
       `CarBrand`.`BrandName` AS `CarBrandName`,
       `InspectionHours`.`Description`,
       `InspectionHours`.`Archived`
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
                    CarLicensePlate = reader.GetString("CarLicensePlate"),
                    Mileage = reader.GetInt32("Mileage"),
                    DateTimeOfInspection = reader.GetDateTime("DateTimeOfInspection"),
                    Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    Archived = reader.GetBoolean("Archived"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, InspectionHoursDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientId",
                DbType = DbType.Int32,
                Value = dto.ClientId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@carId",
                DbType = DbType.Int32,
                Value = dto.CarId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@mileage",
                DbType = DbType.Int32,
                Value = dto.Mileage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateTimeOfInspection",
                DbType = DbType.DateTime,
                Value = dto.DateTimeOfInspection,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = dto.Description,
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
