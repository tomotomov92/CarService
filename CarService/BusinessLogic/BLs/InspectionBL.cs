using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic.BLs
{
    public class InspectionBL : BaseBL<InspectionDTO>, IInspectionBL<InspectionDTO>
    {
        public override string InsertSQL => "INSERT INTO Inspections (ClientId, CarId, Mileage, DateTimeOfInspection, Description, Archived) VALUES (@clientId, @carId, @mileage, @dateTimeOfInspection, @description, @archived);";

        public override string SelectSQL => @"
SELECT Inspections.Id,
       Inspections.ClientId,
       Clients.Id AS Client_Id,
       Clients.FirstName AS Client_FirstName,
       Clients.LastName AS Client_LastName,
       Clients.EmailAddress AS Client_EmailAddress,
       Clients.Archived AS Client_Archived,
       Inspections.CarId,
       ClientCars.Id AS ClientCar_Id,
       ClientCars.ClientId AS ClientCar_ClientId,
       Clients.Id AS ClientCar_Client_Id,
       Clients.FirstName AS ClientCar_Client_FirstName,
       Clients.LastName AS ClientCar_Client_LastName,
       Clients.EmailAddress AS ClientCar_Client_EmailAddress,
       Clients.Archived AS ClientCar_Client_Archived,
       ClientCars.CarBrandId AS ClientCar_CarBrandId,
       CarBrands.Id AS CarBrand_Id,
       CarBrands.BrandName AS CarBrand_BrandName,
       CarBrands.Archived AS CarBrand_Archived,
       ClientCars.LicensePlate AS ClientCar_LicensePlate,
       ClientCars.Mileage AS ClientCar_Mileage,
       ClientCars.Archived AS ClientCar_Archived,
       Inspections.Mileage,
       Inspections.DateTimeOfInspection,
       Inspections.Description,
       Inspections.Archived
FROM Inspections
INNER JOIN Clients ON Clients.Id = Inspections.ClientId
INNER JOIN ClientCars ON ClientCars.Id = Inspections.CarId
INNER JOIN CarBrands ON CarBrands.Id = ClientCars.CarBrandId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Inspections.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Inspections.Archived = 0;";

        public override string UpdateSQL => "UPDATE Inspections SET ClientId = @clientId, CarId = @carId, Mileage = @mileage, DateTimeOfInspection = @dateTimeOfInspection, Description = @description, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Inspections WHERE Id = @id;";

        public string SelectForClientIdSQL => $"{SelectSQL} WHERE Inspections.ClientId = @clientId;";

        public string SelectForCarIdSQL => $"{SelectSQL} WHERE Inspections.CarId = @carId;";

        public InspectionBL(AppDb db)
            : base(db)
        {

        }

        public IEnumerable<InspectionDTO> ReadForClientId(int clientId)
        {
            var results = new List<InspectionDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectForClientIdSQL;
            BindParams(cmd, new InspectionDTO
            {
                ClientId = clientId,
            });
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
            }

            return results;
        }

        public IEnumerable<InspectionDTO> ReadForCarId(int carId)
        {
            var results = new List<InspectionDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectForCarIdSQL;
            BindParams(cmd, new InspectionDTO
            {
                CarId = carId,
            });
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
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

        protected override InspectionDTO BindToObject(MySqlDataReader reader)
        {
            return new InspectionDTO
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
                CarId = reader.GetInt32("CarId"),
                ClientCar = new ClientCarDTO
                {
                    Id = reader.GetInt32("ClientCar_Id"),
                    ClientId = reader.GetInt32("ClientCar_ClientId"),
                    Client = new ClientDTO
                    {
                        Id = reader.GetInt32("ClientCar_Client_Id"),
                        FirstName = reader.GetString("ClientCar_Client_FirstName"),
                        LastName = reader.GetString("ClientCar_Client_LastName"),
                        EmailAddress = reader.GetString("ClientCar_Client_LastName"),
                        Archived = reader.GetBoolean("ClientCar_Client_Archived"),
                    },
                    CarBrandId = reader.GetInt32("ClientCar_CarBrandId"),
                    CarBrand = new CarBrandDTO
                    {
                        Id = reader.GetInt32("CarBrand_Id"),
                        BrandName = reader.GetString("CarBrand_BrandName"),
                        Archived = reader.GetBoolean("CarBrand_Archived"),
                    },
                    LicensePlate = reader.GetString("ClientCar_LicensePlate"),
                    Mileage = reader.GetInt32("ClientCar_Mileage"),
                    Archived = reader.GetBoolean("ClientCar_Archived"),
                },
                Mileage = reader.GetInt32("Mileage"),
                DateTimeOfInspection = reader.GetDateTime("DateTimeOfInspection"),
                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
