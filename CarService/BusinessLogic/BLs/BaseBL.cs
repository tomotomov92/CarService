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
        public AppDb Db { get; set; }

        public BaseBL(AppDb db)
        {
            Db = db;
            Db.Connection.Open();
        }

        public abstract Task<T> AddAsync(T dto);

        public abstract Task<T> UpdateAsync(T dto);

        public abstract T Get(int id);

        public abstract Task DeleteAsync(int id);

        public abstract Task<IEnumerable<T>> GetAllAsync();

        public abstract Task<IEnumerable<T>> GetAllActiveAsync();

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

        public void Dispose()
        {
            Db.Connection.Close();
            Db.Dispose();
        }
    }
}
