using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public abstract class BaseBL<T> : IBaseBL<T>, IDisposable
        where T : IBaseDTO
    {
        public AppDb Db { get; }
        public abstract string InsertSQL { get; }
        public abstract string SelectSQL { get; }
        public abstract string SelectByIdSQL { get; }
        public abstract string SelectActiveSQL { get; }
        public abstract string UpdateSQL { get; }
        public abstract string ArchiveSQL { get; }
        public abstract string DeleteSQL { get; }

        public BaseBL(AppDb db)
        {
            Db = db;
            Db.Connection.Open();
        }

        public virtual async Task<T> CreateAsync(T dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = InsertSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public virtual T ReadById(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectByIdSQL;
            BindId(cmd, id);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            return BindToObject(reader);
        }

        public virtual IEnumerable<T> ReadAll()
        {
            var results = new List<T>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectSQL;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
            }

            return results;
        }

        public virtual IEnumerable<T> ReadActive()
        {
            var results = new List<T>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectActiveSQL;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(BindToObject(reader));
            }

            return results;
        }

        public virtual async Task<T> UpdateAsync(T dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = UpdateSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public virtual async Task<T> ArchiveAsync(T dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = ArchiveSQL;
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public virtual async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = DeleteSQL;
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        protected void BindId(MySqlCommand cmd, int id)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        protected abstract void BindParams(MySqlCommand cmd, T dto);

        protected void BindParams(MySqlCommand cmd, TokenDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = dto.Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientId",
                DbType = DbType.Int32,
                Value = dto.ClientId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@employeeId",
                DbType = DbType.Int32,
                Value = dto.EmployeeId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@token",
                DbType = DbType.String,
                Value = dto.Token,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@expirationDate",
                DbType = DbType.DateTime,
                Value = dto.ExpirationDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@isValid",
                DbType = DbType.Boolean,
                Value = dto.IsValid,
            });
        }

        protected abstract T BindToObject(MySqlDataReader reader);

        protected TokenDTO BindToTokenObject(MySqlDataReader reader)
        {
            return new TokenDTO
            {
                Id = reader.GetInt32("Id"),
                ClientId = reader.IsDBNull("ClientId") ? null : (int?)reader.GetInt32("ClientId"),
                EmployeeId = reader.IsDBNull("EmployeeId") ? null : (int?)reader.GetInt32("EmployeeId"),
                Token = reader.GetString("Token"),
                ExpirationDate = reader.IsDBNull("ExpirationDate") ? null : (DateTime?)reader.GetDateTime("ExpirationDate"),
                IsValid = reader.GetBoolean("IsValid"),
            };
        }

        public void Dispose()
        {
            Db.Connection.Close();
            Db.Dispose();
        }
    }
}
