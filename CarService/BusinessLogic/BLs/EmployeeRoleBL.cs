using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace BusinessLogic.BLs
{
    public class EmployeeRoleBL : BaseBL<EmployeeRoleDTO>
    {
        public override string InsertSQL => "INSERT INTO EmployeeRoles (EmployeeRoleName, Archived) VALUES (@employeeRoleName, @archived);";

        public override string SelectSQL => @"
SELECT EmployeeRoles.Id,
       EmployeeRoles.EmployeeRoleName,
       EmployeeRoles.Archived
FROM EmployeeRoles";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE EmployeeRoles.Id = @id;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE EmployeeRoles.Archived = 0;";

        public override string UpdateSQL => "UPDATE EmployeeRoles SET EmployeeRoleName = @employeeRoleName WHERE Id = @id;";

        public override string ArchiveSQL => "UPDATE EmployeeRoles SET Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM EmployeeRoles WHERE Id = @id;";

        public EmployeeRoleBL(AppDb db)
            : base(db)
        {

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

        protected override EmployeeRoleDTO BindToObject(MySqlDataReader reader)
        {
            return new EmployeeRoleDTO
            {
                Id = reader.GetInt32("Id"),
                EmployeeRoleName = reader.GetString("EmployeeRoleName"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
