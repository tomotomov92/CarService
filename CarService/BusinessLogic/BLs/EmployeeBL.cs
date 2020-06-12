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
            cmd.CommandText = @"INSERT INTO `Employee` (`FirstName`, `LastName`, `EmailAddress`, `Password`, `DateOfStart`, `EmployeeRoleId`, `Archived`) VALUES (@firstName, @lastName, @emailAddress, @password, @dateOfStart, @employeeRoleId, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            var results = new List<EmployeeDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `Employee`.`Id`,
       `Employee`.`FirstName`,
       `Employee`.`LastName`,
       `Employee`.`EmailAddress`,
       `Employee`.`Password`,
       `Employee`.`DateOfStart`,
       `Employee`.`EmployeeRoleId`,
       `EmployeeRoles`.`EmployeeRoleName`,
       `Employee`.`Archived`
FROM `Employee`
INNER JOIN `EmployeeRoles` ON `EmployeeRoles`.`Id` = `Employee`.`EmployeeRoleId`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new EmployeeDTO
                {
                    Id = reader.GetInt32("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    EmailAddress = reader.GetString("EmailAddress"),
                    Password = reader.GetString("Password"),
                    DateOfStart = reader.GetDateTime("DateOfStart"),
                    EmployeeRoleId = reader.GetInt32("EmployeeRoleId"),
                    EmployeeRoleName = reader.GetString("EmployeeRoleName"),
                    Archived = reader.GetBoolean("Archived"),
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
                ParameterName = "@dateOfStart",
                DbType = DbType.DateTime,
                Value = dto.DateOfStart,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@employeeRoleId",
                DbType = DbType.Int32,
                Value = dto.EmployeeRoleId,
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
