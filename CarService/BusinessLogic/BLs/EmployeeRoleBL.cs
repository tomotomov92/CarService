using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class EmployeeRoleBL : BaseBL<EmployeeRoleDTO>
    {
        public EmployeeRoleBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<EmployeeRoleDTO> AddAsync(EmployeeRoleDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO EmployeeRoles (EmployeeRoleName, Archived) VALUES (@employeeRoleName, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<EmployeeRoleDTO> UpdateAsync(EmployeeRoleDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE EmployeeRoles SET EmployeeRoleName = @employeeRoleName, Archived = @archived WHERE Id = @id;";
            BindId(cmd, dto);
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public override EmployeeRoleDTO Get(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT EmployeeRoles.Id,
       EmployeeRoles.EmployeeRoleName,
       EmployeeRoles.Archived
FROM EmployeeRoles
WHERE EmployeeRoles.Id = @id;";
            BindId(cmd, id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new EmployeeRoleDTO
            {
                Id = reader.GetInt32("Id"),
                EmployeeRoleName = reader.GetString("EmployeeRoleName"),
                Archived = reader.GetBoolean("Archived"),
            };
        }

        public override async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM EmployeeRoles WHERE Id = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<EmployeeRoleDTO>> GetAllAsync()
        {
            var results = new List<EmployeeRoleDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT EmployeeRoles.Id,
       EmployeeRoles.EmployeeRoleName,
       EmployeeRoles.Archived
FROM EmployeeRoles;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new EmployeeRoleDTO
                {
                    Id = reader.GetInt32("Id"),
                    EmployeeRoleName = reader.GetString("EmployeeRoleName"),
                    Archived = reader.GetBoolean("Archived"),
                });
            }

            return results;
        }

        public override async Task<IEnumerable<EmployeeRoleDTO>> GetAllActiveAsync()
        {
            var activeResults = await GetAllAsync();
            activeResults = activeResults.Where(x => x.Archived == false);
            return activeResults;
        }

        protected override void BindParams(MySqlCommand cmd, EmployeeRoleDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@employeeRoleName",
                DbType = DbType.String,
                Value = dto.EmployeeRoleName,
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
