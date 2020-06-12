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
            cmd.CommandText = @"INSERT INTO `Schedule` (`DateBegin`, `DateEnd`, `EmployeeId`) VALUES (@dateBegin, @dateEnd, @employeeId);";
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
       `Schedule`.`DateBegin`,
       `Schedule`.`DateEnd`,
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
                    DateBegin = reader.GetDateTime("DateBegin"),
                    DateEnd = reader.GetDateTime("DateEnd"),
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
