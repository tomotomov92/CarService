using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class InspectionBL : BaseBL<InspectionDTO>
    {
        public InspectionBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<InspectionDTO> AddAsync(InspectionDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Inspections (ClientId, CarId, Mileage, DateTimeOfInspection, Description, Archived) VALUES (@clientId, @carId, @mileage, @dateTimeOfInspection, @description, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<InspectionDTO> UpdateAsync(InspectionDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE Inspections SET ClientId = @clientId, CarId = @carId, Mileage = @mileage, DateTimeOfInspection = @dateTimeOfInspection, Description = @description, Archived = @archived WHERE Id = @id;";
            BindId(cmd, dto);
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public override InspectionDTO Get(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
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
INNER JOIN CarBrands ON CarBrands.Id = ClientCars.CarBrandId
WHERE Inspections.Id = @id;";
            BindId(cmd, id);
            var reader = cmd.ExecuteReader();
            reader.Read();
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

        public override async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Inspections WHERE Id = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<InspectionDTO>> GetAllAsync()
        {
            var results = new List<InspectionDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
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
INNER JOIN CarBrands ON CarBrands.Id = ClientCars.CarBrandId
ORDER BY Inspections.Id;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new InspectionDTO
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
                });
            }

            return results;
        }

        public override async Task<IEnumerable<InspectionDTO>> GetAllActiveAsync()
        {
            var activeResults = await GetAllAsync();
            activeResults = activeResults.Where(x => x.Archived == false);
            return activeResults;
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
