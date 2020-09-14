﻿using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class ClientBL : BaseBL<ClientDTO>, ICredentialBL<ClientDTO>
    {
        private readonly EmailBL _emailBl;

        public override string InsertSQL => "INSERT INTO Clients (FirstName, LastName, EmailAddress, Password, RequirePasswordChange, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @requirePasswordChange, @archived);";

        public override string SelectSQL => @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.RequirePasswordChange,
       Clients.Archived
FROM Clients";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Clients.Id = @id;";

        private string SelectByEmailAddressSQL => $"{SelectSQL} WHERE EmailAddress = @emailAddress;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Clients.Archived = 0;";

        public override string UpdateSQL => "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress WHERE Id = @id;";

        public override string ArchiveSQL => "UPDATE Clients SET Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Clients WHERE Id = @id;";

        private string UpdatePasswordSQL => "UPDATE Clients SET Password = @password, RequirePasswordChange = @requirePasswordChange WHERE Id = @id";

        private string InsertClientTokenSQL => "INSERT INTO Tokens (ClientId, Token, ExpirationDate, IsValid) VALUES (@clientId, @token, @expirationDate, @isValid);";

        public ClientBL(AppDb db, EmailBL emailBl)
            : base(db)
        {
            _emailBl = emailBl;
        }

        public async Task<CredentialDTO> RegisterAsync(CredentialDTO dto, string webRootPath)
        {
            var result = new CredentialDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                SuccessfulOperation = false,
            };
            var clientDTO = ReadByEmailAddress(dto.EmailAddress);
            if (clientDTO == null)
            {
                if (dto.Password != dto.RepeatPassword)
                {
                    result.ErrorMessage = Constants.RegistrationPasswordsDoNotMatch;
                }
                else
                {
                    clientDTO = await CreateAsync(new ClientDTO
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        EmailAddress = dto.EmailAddress,
                        Password = dto.HashedPassword,
                    });

                    if (clientDTO.Id > 0)
                    {
                        result.Id = clientDTO.Id;
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
            var clientDTO = ReadByEmailAddress(dto.EmailAddress);
            if (clientDTO == null)
            {
                result.ErrorMessage = Constants.EmailNotExist;
            }
            else if (dto.HashedPassword.Equals(clientDTO.Password))
            {
                result.Id = clientDTO.Id;
                result.FirstName = clientDTO.FirstName;
                result.LastName = clientDTO.LastName;
                result.RequirePasswordChange = clientDTO.RequirePasswordChange;
                result.UserRole = UserRoles.Customer;
                result.SuccessfulOperation = true;
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
            BindParams(cmd, ClientDTO.FromCredentialDTO(dto));
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> ChangePasswordAsync(CredentialDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdatePasswordSQL;
            BindParams(cmd, ClientDTO.FromCredentialDTO(dto));
            var result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<TokenDTO> ForgottenPasswordAsync(CredentialDTO dto, string webRootPath)
        {
            var clientDTO = ReadByEmailAddress(dto.EmailAddress);
            if (clientDTO != null && !clientDTO.Archived)
            {
                var tokenDTO = new TokenDTO
                {
                    ClientId = clientDTO.Id,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    IsValid = true,

                    EmailAddress = clientDTO.EmailAddress,
                    FirstName = clientDTO.FirstName,
                    LastName = clientDTO.LastName,
                    EmailSubject = "Forgotten Password",
                };

                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = InsertClientTokenSQL;
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@clientId",
                    DbType = DbType.Int32,
                    Value = tokenDTO.ClientId,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@token",
                    DbType = DbType.String,
                    Value = tokenDTO.Token,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@expirationDate",
                    DbType = DbType.DateTime,
                    Value = tokenDTO.ExpirationDate,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@isValid",
                    DbType = DbType.Boolean,
                    Value = tokenDTO.IsValid,
                });
                var result = await cmd.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    var emailBody = _emailBl.PopulateHTML(webRootPath, "EmailTemplates//Client_ForgottenPassword.html", new Dictionary<string, string>
                    {
                        { "{FirstName}", tokenDTO.FirstName },
                        { "{EmailAddress}", tokenDTO.EmailAddress },
                        { "{Token}", tokenDTO.Token },
                    });

                    tokenDTO.EmailBody = emailBody;
                    return tokenDTO;
                }
            }
            return null;
        }

        public ClientDTO ReadByEmailAddress(string emailAddress)
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

        protected override void BindParams(MySqlCommand cmd, ClientDTO dto)
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

        protected override ClientDTO BindToObject(MySqlDataReader reader)
        {
            return new ClientDTO
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                EmailAddress = reader.GetString("EmailAddress"),
                Password = reader.GetString("Password"),
                RequirePasswordChange = reader.GetBoolean("RequirePasswordChange"),
                Archived = reader.GetBoolean("Archived"),
            };
        }

        private string GetForgottenPasswordEmailBody(TokenDTO clientTokenDTO)
        {
            return 
"Please confirm your identity to access your eBay account" +
$"Hi {clientTokenDTO.FirstName}," +
"It looks like you’re having trouble signing into your account." +
"Please select the ‘confirm’ button to verify your identity and access your account. (It’s only good for 24 hours.)" +
"<td valign=\"top\" align=\"center\" bgcolor=\"#0079bc\" style=\"background-image:linear-gradient(to bottom,#0079bc 0%,#00519e 100%);background-color:#0079bc;padding:10px 17px;border:0px solid #00519e;border-radius:3px\">"+
$"<a href=\"https://accounts.ebay.com/acctxs/email?id={clientTokenDTO.Token}\" style=\"text-decoration:none;color:#ffffff;font-size:14px;line-height:normal;font-weight:bold;font-family:Helvetica Neue,Helvetica,Arial;padding:10px 15px\" target=\"_blank\"> Confirm </a>"+
"</td> ";
        }

        private string GenerateToken(ClientDTO clientDTO)
        {
            return "";
        }
    }
}
