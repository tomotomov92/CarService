using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class CarBrandBL : BaseBL<CarBrandDTO>
    {
        public CarBrandBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<CarBrandDTO> AddAsync(CarBrandDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `CarBrand` (`BrandName`, `Archived`) VALUES (@brandName, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<CarBrandDTO>> GetAllAsync()
        {
            var results = new List<CarBrandDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `CarBrand`.`Id`,
       `CarBrand`.`BrandName`,
       `CarBrand`.`Archived`
FROM `CarBrand`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new CarBrandDTO
                {
                    Id = reader.GetInt32("Id"),
                    BrandName = reader.GetString("BrandName"),
                    Archived = reader.GetBoolean("Archived"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, CarBrandDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@brandName",
                DbType = DbType.String,
                Value = dto.BrandName,
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
