using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace BusinessLogic.BLs
{
    public class CarBrandBL : BaseBL<CarBrandDTO>
    {
        public override string InsertSQL => "INSERT INTO CarBrands (BrandName, Archived) VALUES (@brandName, @archived);";

        public override string SelectSQL => @"
SELECT CarBrands.Id,
       CarBrands.BrandName,
       CarBrands.Archived
FROM CarBrands";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE CarBrands.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE CarBrands.Archived = 0;";

        public override string UpdateSQL => "UPDATE CarBrands SET BrandName = @brandName, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM CarBrands WHERE Id = @id;";

        public CarBrandBL(AppDb db)
            : base(db)
        {

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

        protected override CarBrandDTO BindToObject(MySqlDataReader reader)
        {
            return new CarBrandDTO
            {
                Id = reader.GetInt32("Id"),
                BrandName = reader.GetString("BrandName"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
