using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class EmployeeBL : BaseBL<EmployeeDTO>
    {
        public EmployeeBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<EmployeeDTO> AddAsync(EmployeeDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Employee` (`FirstName`, `LastName`, `DateOfStart`) VALUES (@firstName, @lastName, @dateOfStart);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            var results = new List<EmployeeDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `FirstName`, `LastName`, `DateOfStart` FROM `Employee`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new EmployeeDTO
                {
                    Id = reader.GetInt32("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    DateOfStart = reader.GetDateTime("DateOfStart"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, EmployeeDTO dto)
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
                ParameterName = "@dateOfStart",
                DbType = DbType.DateTime,
                Value = dto.DateOfStart,
            });
        }
    }
}
