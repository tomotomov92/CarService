using BusinessLogic.BLs.Interfaces;
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
    public class ClientBL : BaseBL<ClientDTO>, ICredentialBL<ClientDTO>
    {
        private readonly EmailBL _emailBl;
        private readonly EmailSender.EmailSender _emailSender;

        public override string InsertSQL => "INSERT INTO Clients (FirstName, LastName, EmailAddress, Password, RequirePasswordChange, Activated, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @requirePasswordChange, @activated, @archived);";

        public override string SelectSQL => @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.RequirePasswordChange,
       Clients.Activated,
       Clients.Archived
FROM Clients";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Clients.Id = @id;";

        private string SelectByEmailAddressSQL => $"{SelectSQL} WHERE EmailAddress = @emailAddress;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Clients.Archived = 0;";

        public override string UpdateSQL => "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress WHERE Id = @id;";

        public override string ArchiveSQL => "UPDATE Clients SET Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Clients WHERE Id = @id;";

        private string UpdatePasswordSQL => "UPDATE Clients SET Password = @password, RequirePasswordChange = @requirePasswordChange WHERE Id = @id";

        public string InsertTokenSQL => "INSERT INTO Tokens (ClientId, Token, ExpirationDate, IsValid) VALUES (@clientId, @token, @expirationDate, @isValid);";

        public string SelectTokenSQL => "SELECT Id, ClientId, EmployeeId, Token, ExpirationDate, IsValid FROM Tokens WHERE ClientId = @clientId;";

        public string UpdateTokenSQL => "UPDATE Tokens SET IsValid = @isValid WHERE Id = @id;";

        public ClientBL(AppDb db, EmailBL emailBl, EmailSender.EmailSender emailSender)
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
                    var transaction = Db.Connection.BeginTransaction();
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

                        var tokenDTO = new TokenDTO
                        {
                            ClientId = clientDTO.Id,
                            ExpirationDate = null,
                            IsValid = true,

                            EmailAddress = clientDTO.EmailAddress,
                            FirstName = clientDTO.FirstName,
                            LastName = clientDTO.LastName,
                            EmailSubject = "Welcome!",
                        };

                        tokenDTO = await CreateTokenAsync(tokenDTO);

                        if (tokenDTO.Id > 0)
                        {
                            var htmlPath = selfRegistration ? "EmailTemplates//Client_Registration_Self.html" : "EmailTemplates//Client_Registration_Assisted.html";
                            var emailBody = _emailBl.PopulateHTML(webRootPath, htmlPath, new Dictionary<string, string>
                            {
                                { "{FirstName}", tokenDTO.FirstName },
                                { "{BASE_URL}", base_url },
                                { "{EmailAddress}", tokenDTO.EmailAddress },
                                { "{Token}", tokenDTO.Token },
                            });

                            tokenDTO.EmailBody = emailBody;
                            _emailSender.SendEmail(tokenDTO.EmailAddress, tokenDTO.EmailSubject, tokenDTO.EmailBody);
                            result.SuccessfulOperation = true;
                            transaction.Commit();
                        }
                        else
                        {
                            result.ErrorMessage = Constants.ErrorDuringTheRegistrationContactSupport;
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        result.ErrorMessage = Constants.ErrorDuringTheRegistrationTryAgain;
                        transaction.Rollback();
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
                if (clientDTO.Archived)
                {
                    result.ErrorMessage = Constants.AccountDeactivatedContactSupport;
                }
                else if(!clientDTO.Activated)
                {
                    result.ErrorMessage = Constants.AccountNotActivated;
                }
                else
                {
                    result.Id = clientDTO.Id;
                    result.FirstName = clientDTO.FirstName;
                    result.LastName = clientDTO.LastName;
                    result.RequirePasswordChange = clientDTO.RequirePasswordChange;
                    result.Activated = clientDTO.Activated;
                    result.Archived = clientDTO.Archived;
                    result.UserRole = UserRoles.Customer;
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

        public async Task<bool> ForgottenPasswordAsync(CredentialDTO dto, string webRootPath, string base_url)
        {
            var clientDTO = ReadByEmailAddress(dto.EmailAddress);
            if (clientDTO != null && !clientDTO.Archived && clientDTO.Activated)
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

                tokenDTO = await CreateTokenAsync(tokenDTO);

                if (tokenDTO.Id > 0)
                {
                    var emailBody = _emailBl.PopulateHTML(webRootPath, "EmailTemplates//Client_ForgottenPassword.html", new Dictionary<string, string>
                    {
                        { "{FirstName}", tokenDTO.FirstName },
                        { "{BASE_URL}", base_url },
                        { "{EmailAddress}", tokenDTO.EmailAddress },
                        { "{Token}", tokenDTO.Token },
                    });

                    tokenDTO.EmailBody = emailBody;
                    _emailSender.SendEmail(tokenDTO.EmailAddress, tokenDTO.EmailSubject, tokenDTO.EmailBody);
                    return true;
                }
            }
            return false;
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

        public async Task<TokenDTO> CreateTokenAsync(TokenDTO dto)
        {
            var existingTokens = ReadTokenByUserId(dto).Where(x => x.IsValid == true);
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
            return ReadTokenByUserId(dto).Where(x => x.IsValid == true);
        }

        public async Task<TokenDTO> UpdateTokenAsync(TokenDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdateTokenSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
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
                ParameterName = "@activated",
                DbType = DbType.Boolean,
                Value = dto.Activated,
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
                Activated = reader.GetBoolean("Activated"),
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
