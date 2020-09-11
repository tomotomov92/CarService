using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace BusinessLogic.BLs
{
    public class ScheduleBL : BaseBL<ScheduleDTO>
    {
        public override string InsertSQL => "INSERT INTO Schedules (DateBegin, DateEnd, EmployeeId) VALUES (@dateBegin, @dateEnd, @employeeId);";

        public override string SelectSQL => @"
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
INNER JOIN EmployeeRoles ON EmployeeRoles.Id = Employees.EmployeeRoleId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Schedules.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Schedules.Archived = 0;";

        public override string UpdateSQL => "UPDATE Schedules SET DateBegin = @dateBegin, DateEnd = @dateEnd, EmployeeId = @employeeId WHERE Id = @id;";

        public override string ArchiveSQL => throw new NotImplementedException();

        public override string DeleteSQL => "DELETE FROM Schedules WHERE Id = @id;";

        public ScheduleBL(AppDb db)
            : base(db)
        {

        }

        protected override void BindParams(MySqlCommand cmd, ScheduleDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = dto.Id,
            });
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

        protected override ScheduleDTO BindToObject(MySqlDataReader reader)
        {
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
    }
}
