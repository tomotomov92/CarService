using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class InspectioBL : BaseBL<InspectionDTO>
    {
        public InspectioBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<InspectionDTO> AddAsync(InspectionDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Inspections` (`ClientId`, `CarId`, `Mileage`, `DateTimeOfInspection`, `Description`, `Archived`) VALUES (@clientId, @carId, @mileage, @dateTimeOfInspection, @description, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<InspectionDTO>> GetAllAsync()
        {
            var results = new List<InspectionDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `Inspections`.`Id`,
       `Inspections`.`ClientId`,
       `Clients`.`FirstName` AS `ClientFirstName`,
       `Clients`.`LastName` AS `ClientLastName`,
       `Inspections`.`CarId`,
       `Inspections`.`Mileage`,
       `Inspections`.`DateTimeOfInspection`,
       `ClientCars`.`LicensePlate` AS `CarLicensePlate`,
       `CarBrands`.`BrandName` AS `CarBrandName`,
       `Inspections`.`Description`,
       `Inspections`.`Archived`
FROM `Inspections`
INNER JOIN `Clients` ON `Clients`.`Id` = `Inspections`.`ClientId`
INNER JOIN `ClientCars` ON `ClientCars`.`Id` = `Inspections`.`CarId`
INNER JOIN `CarBrands` ON `CarBrands`.`Id` = `ClientCars`.`CarBrandId`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new InspectionDTO
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

        protected override void BindParams(MySqlCommand cmd, InspectionDTO dto)
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
