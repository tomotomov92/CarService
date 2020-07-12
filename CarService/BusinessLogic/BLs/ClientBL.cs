using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class ClientBL : BaseBL<ClientDTO>, ICredentialBL<ClientDTO>
    {
        public override string InsertSQL => "INSERT INTO Clients (FirstName, LastName, EmailAddress, Password, Archived) VALUES (@firstName, @lastName, @emailAddress, @password, @archived);";

        public override string SelectSQL => @"
SELECT Clients.Id,
       Clients.FirstName,
       Clients.LastName,
       Clients.EmailAddress,
       Clients.Password,
       Clients.Archived
FROM Clients";

        public override string SelectByIdSQL => $"{SelectSQL} WHERE Clients.Id = @id;";

        private string SelectByEmailAddressSQL => $"{SelectSQL} WHERE EmailAddress = @emailAddress;";

        public override string SelectActiveSQL => $"{SelectSQL} WHERE Clients.Archived = 0;";

        public override string UpdateSQL => "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, Archived = @archived WHERE Id = @id;";

        public override string DeleteSQL => "DELETE FROM Clients WHERE Id = @id;";

        private string UpdateEmailAddressSQL => "Update Clients SET EmailAddress = @emailAddress WHERE Id = @id";

        private string UpdatePasswordSQL => "UPDATE Clients SET Password = @password WHERE Id = @id";

        public ClientBL(AppDb db)
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
                result.UserRole = UserRoles.Customer;
                result.SuccessfulOperation = true;
                if (dto.HashedPassword.Equals(Constants.DefaultPassword))
                {
                    result.RequirePasswordChange = true;
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
            var clientDTO = ReadByEmailAddress(dto.EmailAddress);
            if (clientDTO != null)
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = UpdatePasswordSQL;
                BindId(cmd, clientDTO.Id);
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
                Archived = reader.GetBoolean("Archived"),
            };
        }
    }
}
