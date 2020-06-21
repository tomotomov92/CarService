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
            var reader = cmd.ExecuteReader();
            reader.Read();
            return BindToObject(reader);
        }

        public virtual IEnumerable<T> ReadAll()
        {
            var results = new List<T>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = SelectSQL;
            var reader = cmd.ExecuteReader();
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
            var reader = cmd.ExecuteReader();
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
            BindId(cmd, dto);
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

        protected void BindId(MySqlCommand cmd, T dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = dto.Id,
            });
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

        protected abstract T BindToObject(MySqlDataReader reader);

        public void Dispose()
        {
            Db.Connection.Close();
            Db.Dispose();
        }
    }
}
