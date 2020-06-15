﻿using BusinessLogic.DTOs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.BLs
{
    public class CarBrandBL : BaseBL<CarBrandDTO>
    {
        public CarBrandBL(AppDb db)
            : base(db)
        {

        }

        public override async Task<CarBrandDTO> AddAsync(CarBrandDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO CarBrands (BrandName, Archived) VALUES (@brandName, @archived);";
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            dto.Id = (int)cmd.LastInsertedId;
            return dto;
        }

        public override async Task<CarBrandDTO> UpdateAsync(CarBrandDTO dto)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE CarBrands SET BrandName = @brandName, Archived = @archived WHERE Id = @id;";
            BindId(cmd, dto);
            BindParams(cmd, dto);
            await cmd.ExecuteNonQueryAsync();
            return dto;
        }

        public override CarBrandDTO Get(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT CarBrands.Id,
       CarBrands.BrandName,
       CarBrands.Archived
FROM CarBrands
WHERE CarBrands.Id = @id;";
            BindId(cmd, id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new CarBrandDTO
            {
                Id = reader.GetInt32("Id"),
                BrandName = reader.GetString("BrandName"),
                Archived = reader.GetBoolean("Archived"),
            };
        }

        public override async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM CarBrands WHERE Id = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<CarBrandDTO>> GetAllAsync()
        {
            var results = new List<CarBrandDTO>();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"
SELECT CarBrands.Id,
       CarBrands.BrandName,
       CarBrands.Archived
FROM CarBrands;";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new CarBrandDTO
                {
                    Id = reader.GetInt32("Id"),
                    BrandName = reader.GetString("BrandName"),
                    Archived = reader.GetBoolean("Archived"),
                });
            }

            return results;
        }

        public override async Task<IEnumerable<CarBrandDTO>> GetAllActiveAsync()
        {
            var activeResults = await GetAllAsync();
            activeResults = activeResults.Where(x => x.Archived == false);
            return activeResults;
        }

        protected override void BindParams(MySqlCommand cmd, CarBrandDTO dto)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@brandName",
                DbType = DbType.String,
                Value = dto.BrandName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@archived",
                DbType = DbType.Boolean,
                Value = dto.Archived,
            });
        }
    }
}
