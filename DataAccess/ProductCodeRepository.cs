using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 产品条码DAO
    public class ProductCodeRepository : BaseRepository
    {
        public ProductCodeRepository(string connectionString) : base(connectionString) { }

        public List<ProductCode> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<ProductCode>("SELECT * FROM ProductCode ORDER BY Id").ToList();
        }

        public ProductCode GetByDisplayCode(string displayCode)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<ProductCode>(
                    "SELECT * FROM ProductCode WHERE DisplayCode = @DisplayCode",
                    new { DisplayCode = displayCode });
        }

        public void Add(ProductCode code)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO ProductCode (DisplayCode, ConvertedCode) VALUES (@DisplayCode, @ConvertedCode)", code);
        }

        public ProductCode GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<ProductCode>("SELECT * FROM ProductCode WHERE Id = @Id", new { Id = id });
        }

        public void Update(ProductCode productCode, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE ProductCode SET DisplayCode = @DisplayCode, ConvertedCode = @ConvertedCode WHERE Id = @Id", new { productCode.DisplayCode, productCode.ConvertedCode, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM ProductCode WHERE Id = @Id", new { Id = id });
        }

        public bool Exists(string displayCode, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM ProductCode WHERE DisplayCode = @DisplayCode";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { DisplayCode = displayCode, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
