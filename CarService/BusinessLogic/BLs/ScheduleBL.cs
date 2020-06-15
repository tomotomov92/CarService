using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class ScheduleBL : BaseBL<ScheduleDTO>
    {
        public ScheduleBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<ScheduleDTO> AddAsync(ScheduleDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Schedules (DateBegin, DateEnd, EmployeeId) VALUES (@dateBegin, @dateEnd, @employeeId);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<ScheduleDTO> UpdateAsync(ScheduleDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE Schedules SET DateBegin = @dateBegin, DateEnd = @dateEnd, EmployeeId = @employeeId WHERE Id = @id;";
            BindId(cmd, dto);
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public override ScheduleDTO Get(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT Schedules.Id,
       Schedules.DateBegin,
       Schedules.DateEnd,
       Schedules.EmployeeId,
       Employees.Id AS Employee_Id,
       Employees.FirstName AS Employee_FirstName,
       Employees.LastName AS Employee_LastName,
       Employees.EmailAddress AS Employee_EmailAddress,
       Employees.DateOfStart AS Employee_DateOfStart,
       Employees.EmployeeRoleId AS Employee_EmployeeRoleId,
       EmployeeRoles.Id AS EmployeeRole_Id,
       EmployeeRoles.EmployeeRoleName AS EmployeeRole_EmployeeRoleName,
       EmployeeRoles.Archived AS EmployeeRole_Archived,
       Employees.Archived AS Employee_Archived
FROM Schedules
INNER JOIN Employees ON Schedules.EmployeeId = Employees.Id
INNER JOIN EmployeeRoles ON EmployeeRoles.Id = Employees.EmployeeRoleId
WHERE Schedules.Id = @id;";
            BindId(cmd, id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new ScheduleDTO
            {
                Id = reader.GetInt32("Id"),
                DateBegin = reader.GetDateTime("DateBegin"),
                DateEnd = reader.GetDateTime("DateEnd"),
                EmployeeId = reader.GetInt32("EmployeeId"),
                Employee = new EmployeeDTO
                {
                    Id = reader.GetInt32("Employee_Id"),
                    FirstName = reader.GetString("Employee_FirstName"),
                    LastName = reader.GetString("Employee_LastName"),
                    EmailAddress = reader.GetString("Employee_EmailAddress"),
                    DateOfStart = reader.GetDateTime("Employee_DateOfStart"),
                    EmployeeRoleId = reader.GetInt32("Employee_EmployeeRoleId"),
                    EmployeeRole = new EmployeeRoleDTO
                    {
                        Id = reader.GetInt32("EmployeeRole_Id"),
                        EmployeeRoleName = reader.GetString("EmployeeRole_EmployeeRoleName"),
                        Archived = reader.GetBoolean("EmployeeRole_Archived"),
                    },
                    Archived = reader.GetBoolean("Employee_Archived"),
                }
            };
        }

        public override async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Schedules WHERE Id = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<ScheduleDTO>> GetAllAsync()
        {
            var results = new List<ScheduleDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT Schedules.Id,
       Schedules.DateBegin,
       Schedules.DateEnd,
       Schedules.EmployeeId,
       Employees.Id AS Employee_Id,
       Employees.FirstName AS Employee_FirstName,
       Employees.LastName AS Employee_LastName,
       Employees.EmailAddress AS Employee_EmailAddress,
       Employees.DateOfStart AS Employee_DateOfStart,
       Employees.EmployeeRoleId AS Employee_EmployeeRoleId,
       EmployeeRoles.Id AS EmployeeRole_Id,
       EmployeeRoles.EmployeeRoleName AS EmployeeRole_EmployeeRoleName,
       EmployeeRoles.Archived AS EmployeeRole_Archived,
       Employees.Archived AS Employee_Archived
FROM Schedules
INNER JOIN Employees ON Schedules.EmployeeId = Employees.Id
INNER JOIN EmployeeRoles ON EmployeeRoles.Id = Employees.EmployeeRoleId
ORDER BY Schedules.Id;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ScheduleDTO
                {
                    Id = reader.GetInt32("Id"),
                    DateBegin = reader.GetDateTime("DateBegin"),
                    DateEnd = reader.GetDateTime("DateEnd"),
                    EmployeeId = reader.GetInt32("EmployeeId"),
                    Employee = new EmployeeDTO
                    {
                        Id = reader.GetInt32("Employee_Id"),
                        FirstName = reader.GetString("Employee_FirstName"),
                        LastName = reader.GetString("Employee_LastName"),
                        EmailAddress = reader.GetString("Employee_EmailAddress"),
                        DateOfStart = reader.GetDateTime("Employee_DateOfStart"),
                        EmployeeRoleId = reader.GetInt32("Employee_EmployeeRoleId"),
                        EmployeeRole = new EmployeeRoleDTO
                        {
                            Id = reader.GetInt32("EmployeeRole_Id"),
                            EmployeeRoleName = reader.GetString("EmployeeRole_EmployeeRoleName"),
                            Archived = reader.GetBoolean("EmployeeRole_Archived"),
                        },
                        Archived = reader.GetBoolean("Employee_Archived"),
                    }
                });
            }

            return results;
        }

        public override async Task<IEnumerable<ScheduleDTO>> GetAllActiveAsync()
        {
            var activeResults = await GetAllAsync();
            return activeResults;
        }

        protected override void BindParams(MySqlCommand cmd, ScheduleDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateBegin",
                DbType = DbType.DateTime,
                Value = dto.DateBegin,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateEnd",
                DbType = DbType.DateTime,
                Value = dto.DateEnd,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@employeeId",
                DbType = DbType.Int32,
                Value = dto.EmployeeId,
            });
        }
    }
}
