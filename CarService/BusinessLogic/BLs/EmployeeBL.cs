using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class EmployeeBL : BaseBL<EmployeeDTO>, ICredentialBL<EmployeeDTO>
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

        private string SelectByEmailAddressSQL => $"{SelectSQL} WHERE EmailAddress = @emailAddress;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Employees.Archived = 0;";

        public override string UpdateSQL => "UPDATE Employees SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, EmployeeRoleId = @employeeRoleId, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Employees WHERE Id = @id;";

        private string UpdateEmailAddressSQL => "Update Employees SET EmailAddress = @emailAddress WHERE Id = @id";

        private string UpdatePasswordSQL => "UPDATE Employees SET Password = @password WHERE Id = @id";

        public EmployeeBL(AppDb db)
            : base(db)
        {

        }

        public async Task<CredentialDTO> RegisterAsync(CredentialDTO dto)
        {
            var result = new CredentialDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                SuccessfulOperation = false,
            };
            var employeeDTO = ReadByEmailAddress(dto.EmailAddress);
            if (employeeDTO == null)
            {
                if (dto.Password != dto.RepeatPassword)
                {
                    result.ErrorMessage = Constants.RegistrationPasswordsDoNotMatch;
                }
                else
                {
                    employeeDTO = await CreateAsync(new EmployeeDTO
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        EmailAddress = dto.EmailAddress,
                        Password = dto.HashedPassword,
                    });

                    if (employeeDTO.Id > 0)
                    {
                        result.Id = employeeDTO.Id;
                        result.SuccessfulOperation = true;
                    }
                    else
                    {
                        result.ErrorMessage = Constants.ErrorDuringTheRegistrationTryAgain;
                    }
                }
            }
            else
                result.ErrorMessage = Constants.UserAlreadyExists;
            return result;
        }

        public CredentialDTO LogIn(CredentialDTO dto)
        {
            var result = new CredentialDTO
            {
                EmailAddress = dto.EmailAddress,
                SuccessfulOperation = false,
            };
            var employeeDTO = ReadByEmailAddress(dto.EmailAddress);
            if (employeeDTO == null)
            {
                result.ErrorMessage = Constants.EmailNotExist;
            }
            else if (dto.HashedPassword.Equals(employeeDTO.Password))
            {
                result.Id = employeeDTO.Id;
                result.FirstName = employeeDTO.FirstName;
                result.LastName = employeeDTO.LastName;
                result.UserRole = (UserRoles)employeeDTO.EmployeeRoleId;
                result.SuccessfulOperation = true;
                if (dto.HashedPassword.Equals(Constants.DefaultPassword))
                {
                    //result.RequirePasswordChange = true;
                }
            }
            else
            {
                result.ErrorMessage = Constants.WrongPassword;
            }
            return result;
        }

        public async Task<bool> ChangeEmailAddressAsync(CredentialDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdatePasswordSQL;
            BindId(cmd, dto.Id);
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@emailAddress",
                DbType = DbType.String,
                Value = dto.EmailAddress,
            });
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> ChangePasswordAsync(CredentialDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdatePasswordSQL;
            BindId(cmd, dto.Id);
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = dto.HashedPassword,
            });
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> ForgottenPasswordAsync(CredentialDTO dto)
        {
            var employeeDTO = ReadByEmailAddress(dto.EmailAddress);
            if (employeeDTO != null)
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = UpdatePasswordSQL;
                BindId(cmd, employeeDTO.Id);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@password",
                    DbType = DbType.String,
                    Value = Constants.DefaultPassword,
                });
                await cmd.ExecuteNonQueryAsync();
                var result = await cmd.ExecuteNonQueryAsync();
                if (result > 0)
                    return true;
            }
           return false;
        }

        public EmployeeDTO ReadByEmailAddress(string emailAddress)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectByEmailAddressSQL;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@emailAddress",
                DbType = DbType.String,
                Value = emailAddress,
            });
            using var reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
                return BindToObject(reader);
            else
                return null;
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
