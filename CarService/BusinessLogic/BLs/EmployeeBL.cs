﻿using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class EmployeeBL : BaseBL<EmployeeDTO>, ICredentialBL<EmployeeDTO>
    {
        private readonly EmailBL _emailBl;
        private readonly EmailSender.EmailSender _emailSender;

        public override string InsertSQL => "INSERT INTO Employees (FirstName, LastName, EmailAddress, Password, DateOfStart, EmployeeRoleId, RequirePasswordChange, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @dateOfStart, @employeeRoleId, @requirePasswordChange, @archived);";

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
       Employees.RequirePasswordChange,
       Employees.Archived
FROM Employees
INNER JOIN EmployeeRoles ON EmployeeRoles.Id = Employees.EmployeeRoleId";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Employees.Id = @id;";

        private string SelectByEmailAddressSQL => $"{SelectSQL} WHERE EmailAddress = @emailAddress;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Employees.Archived = 0;";

        public override string UpdateSQL => "UPDATE Employees SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, EmployeeRoleId = @employeeRoleId WHERE Id = @id;";

        public override string ArchiveSQL => "UPDATE Employees SET Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Employees WHERE Id = @id;";

        private string UpdatePasswordSQL => "UPDATE Employees SET Password = @password, RequirePasswordChange = @requirePasswordChange WHERE Id = @id";

        public string ConfirmAccountSQL => "UPDATE Employees SET Activated = @activated WHERE Id = @id";

        public string InsertTokenSQL => "INSERT INTO Tokens (EmployeeId, Token, ExpirationDate, IsValid) VALUES (@employeeId, @token, @expirationDate, @isValid);";

        public string SelectTokenSQL => "SELECT Id, ClientId, EmployeeId, Token, ExpirationDate, IsValid FROM Tokens WHERE EmployeeId = @employeeId;";

        public string UpdateTokenSQL => "UPDATE Tokens SET IsValid = @isValid WHERE Id = @id;";

        public EmployeeBL(AppDb db, EmailBL emailBl, EmailSender.EmailSender emailSender)
            : base(db)
        {
            _emailBl = emailBl;
            _emailSender = emailSender;
        }

        public async Task<CredentialDTO> RegisterAsync(CredentialDTO dto, string webRootPath, string base_url, bool selfRegistration)
        {
            var result = new CredentialDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
                EmployeeRole = dto.EmployeeRole,
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
                        DateOfStart = dto.DateOfStart,
                        EmployeeRoleId = dto.EmployeeRoleId,
                    });

                    if (employeeDTO.Id > 0)
                    {
                        result.Id = employeeDTO.Id;

                        var tokenDTO = new TokenDTO
                        {
                            EmailAddress = employeeDTO.EmailAddress,
                            FirstName = employeeDTO.FirstName,
                            LastName = employeeDTO.LastName,
                            EmailSubject = "Welcome to our team!",
                        };

                        var emailBody = _emailBl.PopulateHTML(webRootPath, "EmailTemplates//Employee_Registration.html", new Dictionary<string, string>
                        {
                            { "{FirstName}", tokenDTO.FirstName },
                            { "{BASE_URL}", base_url },
                            { "{EmailAddress}", tokenDTO.EmailAddress },
                            { "{Token}", tokenDTO.Token },
                        });

                        tokenDTO.EmailBody = emailBody;
                        _emailSender.SendEmail(tokenDTO.EmailAddress, tokenDTO.EmailSubject, tokenDTO.EmailBody);
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
                if (employeeDTO.Archived)
                {
                    result.ErrorMessage = Constants.AccountDeactivatedContactSupport;
                }
                else
                {
                    result.Id = employeeDTO.Id;
                    result.FirstName = employeeDTO.FirstName;
                    result.LastName = employeeDTO.LastName;
                    result.RequirePasswordChange = employeeDTO.RequirePasswordChange;
                    result.UserRole = (UserRoles)employeeDTO.EmployeeRoleId;
                    result.SuccessfulOperation = true;
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
            BindParams(cmd, EmployeeDTO.FromCredentialDTO(dto));
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> ChangePasswordAsync(CredentialDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdatePasswordSQL;
            BindParams(cmd, EmployeeDTO.FromCredentialDTO(dto));
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> ForgottenPasswordAsync(CredentialDTO dto, string webRootPath, string base_url)
        {
            var employeeDTO = ReadByEmailAddress(dto.EmailAddress);
            if (employeeDTO != null && !employeeDTO.Archived)
            {
                var tokenDTO = new TokenDTO
                {
                    EmployeeId = employeeDTO.Id,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    IsValid = true,

                    EmailAddress = employeeDTO.EmailAddress,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    EmailSubject = "Forgotten Password",
                };

                tokenDTO = await CreateTokenAsync(tokenDTO);

                if (tokenDTO.Id > 0)
                {
                    var emailBody = _emailBl.PopulateHTML(webRootPath, "EmailTemplates//Employee_ForgottenPassword.html", new Dictionary<string, string>
                    {
                        { "{FirstName}", tokenDTO.FirstName },
                        { "{BASE_URL}", base_url },
                        { "{EmailAddress}", tokenDTO.EmailAddress },
                        { "{Token}", tokenDTO.Token },
                    });

                    tokenDTO.EmailBody = emailBody;
                    _emailSender.SendEmail(tokenDTO.EmailAddress, tokenDTO.EmailSubject, tokenDTO.EmailBody);
                    return true;
                };
            }
            return false;
        }

        public async Task<bool> ConfirmAccountAsync(EmployeeDTO dto, TokenDTO tokenDTO)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdateTokenSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();

            tokenDTO.IsValid = false;
            await UpdateTokenAsync(tokenDTO);
            return true;
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

        public async Task<TokenDTO> CreateTokenAsync(TokenDTO dto)
        {
            var existingTokens = ReadActiveTokenByUserId(dto);
            foreach (var tokenDTO in existingTokens)
            {
                tokenDTO.IsValid = false;
                await UpdateTokenAsync(tokenDTO);
            }

            using var insertCmd = Db.Connection.CreateCommand();
            insertCmd.CommandText = InsertTokenSQL;
            BindParams(insertCmd, dto);
            var insertResult = await insertCmd.ExecuteNonQueryAsync();
            if (insertResult > 0)
            {
                dto.Id = (int)insertCmd.LastInsertedId;
            }
            return dto;
        }

        public IEnumerable<TokenDTO> ReadTokenByUserId(TokenDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectTokenSQL;
            BindParams(cmd, dto);
            using var reader = cmd.ExecuteReader();

            var tokens = new List<TokenDTO>();
            while (reader.Read())
            {
                var tokenDTO = BindToTokenObject(reader);
                tokens.Add(tokenDTO);
            }
            reader.Close();
            return tokens;
        }

        public IEnumerable<TokenDTO> ReadActiveTokenByUserId(TokenDTO dto)
        {
            return ReadTokenByUserId(dto).Where(x => x.IsValid == true && (x.ExpirationDate == null || x.ExpirationDate >= DateTime.Now));
        }

        public async Task<TokenDTO> UpdateTokenAsync(TokenDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdateTokenSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        protected override void BindParams(MySqlCommand cmd, EmployeeDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = dto.Id,
            });
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
                ParameterName = "@requirePasswordChange",
                DbType = DbType.Boolean,
                Value = dto.RequirePasswordChange,
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
                RequirePasswordChange = reader.GetBoolean("RequirePasswordChange"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
