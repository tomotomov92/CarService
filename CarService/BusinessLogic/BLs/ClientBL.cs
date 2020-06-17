using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace BusinessLogic.BLs
{
    public class ClientBL : BaseBL<ClientDTO>
    {
        public override string InsertSQL => "INSERT INTO Clients (FirstName, LastName, EmailAddress, Password, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @archived);";

        public override string SelectSQL => @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.Archived
FROM Clients";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Clients.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Clients.Archived = 0;";

        public override string UpdateSQL => "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Clients WHERE Id = @id;";

        public ClientBL(AppDb db)
            : base(db)
        {

        }

        protected override void BindParams(MySqlCommand cmd, ClientDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = dto.FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = dto.LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@emailAddress",
                DbType = DbType.String,
                Value = dto.EmailAddress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = dto.Password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@archived",
                DbType = DbType.Boolean,
                Value = dto.Archived,
            });
        }

        protected override ClientDTO BindToObject(MySqlDataReader reader)
        {
            return new ClientDTO
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                EmailAddress = reader.GetString("EmailAddress"),
                Password = reader.GetString("Password"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
