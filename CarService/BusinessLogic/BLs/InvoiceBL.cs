using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic.BLs
{
    public class InvoiceBL : BaseBL<InvoiceDTO>, IInvoiceBL<InvoiceDTO>
    {
        public override string InsertSQL => "INSERT INTO Invoices (InspectionId, InvoiceDate, InvoiceSum, Description, Archived) VALUES (@inspectionId, @invoiceDate, @invoiceSum, @description, @archived);";

        public override string SelectSQL => @"
SELECT Invoices.Id,
       Invoices.InspectionId,
       Inspections.Id AS Inspection_Id,
       Inspections.ClientId AS Inspection_ClientId,
       Clients.Id AS Client_Id,
       Clients.FirstName AS Client_FirstName,
       Clients.LastName AS Client_LastName,
       Clients.EmailAddress AS Client_EmailAddress,
       Clients.Archived AS Client_Archived,
       Inspections.CarId AS Inspection_CarId,
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
       Inspections.Mileage AS Inspection_Mileage,
       Inspections.DateTimeOfInspection AS Inspection_DateTimeOfInspection,
       Inspections.Description AS Inspection_Description,
       Inspections.Archived AS Inspection_Archived,
       Invoices.InvoiceDate,
       Invoices.InvoiceSum,
       Invoices.Description,
       Invoices.Archived
FROM Invoices
INNER JOIN Inspections ON Inspections.Id = Invoices.InspectionId
INNER JOIN Clients ON Clients.Id = Inspections.ClientId
INNER JOIN ClientCars ON ClientCars.Id = Inspections.CarId
INNER JOIN CarBrands ON CarBrands.Id = ClientCars.CarBrandId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Invoices.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Invoices.Archived = 0;";

        public override string UpdateSQL => "UPDATE Invoices SET InspectionId = @inspectionId, InvoiceDate = @invoiceDate, InvoiceSum = @invoiceSum, Description = @description, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Invoices WHERE Id = @id;";

        public string SelectForClientIdSQL => $"{SelectSQL} WHERE Inspections.ClientId = @clientId;";

        public InvoiceBL(AppDb db)
            : base(db)
        {

        }

        public IEnumerable<InvoiceDTO> ReadForClientId(int clientId)
        {
            var results = new List<InvoiceDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectForClientIdSQL;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientId",
                DbType = DbType.Int32,
                Value = clientId,
            });
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, InvoiceDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@inspectionId",
                DbType = DbType.Int32,
                Value = dto.InspectionId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@invoiceDate",
                DbType = DbType.DateTime,
                Value = dto.InvoiceDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@invoiceSum",
                DbType = DbType.Int32,
                Value = dto.InvoiceSum,
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

        protected override InvoiceDTO BindToObject(MySqlDataReader reader)
        {
            return new InvoiceDTO
            {
                Id = reader.GetInt32("Id"),
                InspectionId = reader.GetInt32("InspectionId"),
                Inspection = new InspectionDTO
                {
                    Id = reader.GetInt32("Inspection_Id"),
                    ClientId = reader.GetInt32("Inspection_ClientId"),
                    Client = new ClientDTO
                    {
                        Id = reader.GetInt32("Client_Id"),
                        FirstName = reader.GetString("Client_FirstName"),
                        LastName = reader.GetString("Client_LastName"),
                        EmailAddress = reader.GetString("Client_LastName"),
                        Archived = reader.GetBoolean("Client_Archived"),
                    },
                    CarId = reader.GetInt32("Inspection_CarId"),
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
                    Mileage = reader.GetInt32("Inspection_Mileage"),
                    DateTimeOfInspection = reader.GetDateTime("Inspection_DateTimeOfInspection"),
                    Description = reader.IsDBNull("Inspection_Description") ? null : reader.GetString("Inspection_Description"),
                    Archived = reader.GetBoolean("Inspection_Archived"),
                },
                InvoiceDate = reader.GetDateTime("InvoiceDate"),
                InvoiceSum = reader.GetDecimal("InvoiceSum"),
                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
