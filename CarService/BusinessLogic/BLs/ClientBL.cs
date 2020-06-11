using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
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
            cmd.CommandText = @"INSERT INTO `Client` (`FirstName`, `LastName`) VALUES (@firstName, @lastName);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var results = new List<ClientDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `Client`.`Id`,
       `Client`.`FirstName`,
       `Client`.`LastName`
FROM `Client`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ClientDTO
                {
                    Id = reader.GetInt32("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                });
            }

            return results;
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
        }
    }
}
