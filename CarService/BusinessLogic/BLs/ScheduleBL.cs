using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
            cmd.CommandText = @"INSERT INTO `Schedule` (`ForDate`, `HourBegin`, `HourEnd`, `EmployeeId`) VALUES (@forDate, @hourBegin, @hourEnd, @employeeId);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<IEnumerable<ScheduleDTO>> GetAllAsync()
        {
            var results = new List<ScheduleDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT `Schedule`.`Id`,
       `Schedule`.`ForDate`,
       `Schedule`.`HourBegin`,
       `Schedule`.`HourEnd`,
       `Schedule`.`EmployeeId`,
       `Employee`.`FirstName` AS `EmployeeFirstName`,
       `Employee`.`LastName` AS `EmployeeLastName`
FROM `Schedule`
INNER JOIN `Employee` ON `Schedule`.`EmployeeId` = `Employee`.`Id`;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ScheduleDTO
                {
                    Id = reader.GetInt32("Id"),
                    ForDate = reader.GetDateTime("ForDate"),
                    HourBegin = reader.GetDecimal("HourBegin"),
                    HourEnd = reader.GetDecimal("HourEnd"),
                    EmployeeId = reader.GetInt32("EmployeeId"),
                    EmployeeFirstName = reader.GetString("EmployeeFirstName"),
                    EmployeeLastName = reader.GetString("EmployeeLastName"),
                });
            }

            return results;
        }

        protected override void BindParams(MySqlCommand cmd, ScheduleDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@forDate",
                DbType = DbType.DateTime,
                Value = dto.ForDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hourBegin",
                DbType = DbType.Decimal,
                Value = dto.HourBegin,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hourEnd",
                DbType = DbType.Decimal,
                Value = dto.HourEnd,
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
