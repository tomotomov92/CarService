using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class ClientBL : BaseBL<ClientDTO>
    {
        public ClientBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<ClientDTO> AddAsync(ClientDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Clients (FirstName, LastName, EmailAddress, Password, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<ClientDTO> UpdateAsync(ClientDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE Clients SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, Password = @password, Archived = @archived WHERE Id = @id;";
            BindId(cmd, dto);
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public override ClientDTO Get(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.Archived
FROM Clients
WHERE Clients.Id = @id;";
            BindId(cmd, id);
            var reader = cmd.ExecuteReader();
            reader.Read();
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

        public override async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Clients WHERE Id = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var results = new List<ClientDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.Archived
FROM Clients;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ClientDTO
                {
                    Id = reader.GetInt32("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    EmailAddress = reader.GetString("EmailAddress"),
                    Password = reader.GetString("Password"),
                    Archived = reader.GetBoolean("Archived"),
                });
            }

            return results;
        }

        public override async Task<IEnumerable<ClientDTO>> GetAllActiveAsync()
        {
            var activeResults = await GetAllAsync();
            activeResults = activeResults.Where(x => x.Archived == false);
            return activeResults;
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
                Value = dto.FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = dto.LastName,
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
