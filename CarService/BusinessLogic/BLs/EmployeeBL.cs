using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace BusinessLogic.BLs
{
    public class EmployeeBL : BaseBL<EmployeeDTO>
    {
        public override string InsertSQL => "INSERT INTO Employees (FirstName, LastName, EmailAddress, Password, DateOfStart, EmployeeRoleId, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @dateOfStart, @employeeRoleId, @archived);";

        public override string SelectSQL => @"
SELECT Employees.Id,
       Employees.FirstName,
       Employees.LastName,
       Employees.EmailAddress,
       Employees.Password,
       Employees.DateOfStart,
       Employees.EmployeeRoleId,
       EmployeeRoles.Id AS EmployeeRole_Id,
       EmployeeRoles.EmployeeRoleName AS EmployeeRole_EmployeeRoleName,
       EmployeeRoles.Archived AS EmployeeRole_Archived,
       Employees.Archived
FROM Employees
INNER JOIN EmployeeRoles ON EmployeeRoles.Id = Employees.EmployeeRoleId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Employees.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Employees.Archived = 0;";

        public override string UpdateSQL => "UPDATE Employees SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, EmployeeRoleId = @employeeRoleId, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Employees WHERE Id = @id;";

        public EmployeeBL(AppDb db)
            : base(db)
        {

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

        protected override EmployeeDTO BindToObject(MySqlDataReader reader)
        {
            return new EmployeeDTO
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                EmailAddress = reader.GetString("EmailAddress"),
                Password = reader.GetString("Password"),
                DateOfStart = reader.GetDateTime("DateOfStart"),
                EmployeeRoleId = reader.GetInt32("EmployeeRoleId"),
                EmployeeRole = new EmployeeRoleDTO
                {
                    Id = reader.GetInt32("EmployeeRole_Id"),
                    EmployeeRoleName = reader.GetString("EmployeeRole_EmployeeRoleName"),
                    Archived = reader.GetBoolean("EmployeeRole_Archived"),
                },
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
