using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic.BLs
{
    public class ClientCarBL : BaseBL<ClientCarDTO>, IClientCarBL<ClientCarDTO>
    {
        public override string InsertSQL => "INSERT INTO ClientCars (ClientId, CarBrandId, LicensePlate, Mileage, Archived) VALUES (@clientId, @carBrandId, @licensePlate, @mileage, @archived);";

        public override string SelectSQL => @"
SELECT ClientCars.Id,
       ClientCars.ClientId,
       Clients.Id AS Client_Id,
       Clients.FirstName AS Client_FirstName,
       Clients.LastName AS Client_LastName,
       Clients.EmailAddress AS Client_EmailAddress,
       Clients.Archived AS Client_Archived,
       ClientCars.CarBrandId,
       CarBrands.Id AS CarBrand_Id,
       CarBrands.BrandName AS CarBrand_BrandName,
       CarBrands.Archived AS CarBrand_Archived,
       ClientCars.LicensePlate,
       ClientCars.Mileage,
       ClientCars.Archived
FROM ClientCars
INNER JOIN Clients ON Clients.Id = ClientCars.ClientId
INNER JOIN CarBrands ON CarBrands.Id = ClientCars.CarBrandId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE ClientCars.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE ClientCars.Archived = 0;";

        public override string UpdateSQL => "UPDATE ClientCars SET ClientId = @clientId, CarBrandId = @carBrandId, LicensePlate = @licensePlate, Mileage = @mileage, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM ClientCars WHERE Id = @id;";

        public string SelectForClientIdSQL => $"{SelectSQL} WHERE ClientCars.ClientId = @clientId;";

        public ClientCarBL(AppDb db)
            : base(db)
        {

        }
        public IEnumerable<ClientCarDTO> ReadForClientId(int clientId)
        {
            var results = new List<ClientCarDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectForClientIdSQL;
            BindParams(cmd, new ClientCarDTO
            {
                ClientId = clientId,
            });
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
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

        protected override ClientCarDTO BindToObject(MySqlDataReader reader)
        {
            return new ClientCarDTO
            {
                Id = reader.GetInt32("Id"),
                ClientId = reader.GetInt32("ClientId"),
                Client = new ClientDTO
                {
                    Id = reader.GetInt32("Client_Id"),
                    FirstName = reader.GetString("Client_FirstName"),
                    LastName = reader.GetString("Client_LastName"),
                    EmailAddress = reader.GetString("Client_LastName"),
                    Archived = reader.GetBoolean("Client_Archived"),
                },
                CarBrandId = reader.GetInt32("CarBrandId"),
                CarBrand = new CarBrandDTO
                {
                    Id = reader.GetInt32("CarBrand_Id"),
                    BrandName = reader.GetString("CarBrand_BrandName"),
                    Archived = reader.GetBoolean("CarBrand_Archived"),
                },
                LicensePlate = reader.GetString("LicensePlate"),
                Mileage = reader.GetInt32("Mileage"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
