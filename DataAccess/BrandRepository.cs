using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 品牌DAO
    public class BrandRepository : BaseRepository
    {
        public BrandRepository(string connectionString) : base(connectionString) { }

        public List<Brand> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<Brand>("SELECT * FROM Brand ORDER BY Id").ToList();
        }

        public Brand GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Brand>("SELECT * FROM Brand WHERE Id = @Id", new { Id = id });
        }

        public void Add(Brand brand)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO Brand (DisplayName, Code) VALUES (@DisplayName, @Code)", brand);
        }

        public void Update(Brand brand, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE Brand SET DisplayName = @DisplayName, Code = @Code WHERE Id = @Id", new { brand.DisplayName, brand.Code, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM Brand WHERE Id = @Id", new { Id = id });
        }

        public bool Exists(string code, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM Brand WHERE Code = @Code";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { Code = code, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
