using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 规格转换DAO
    public class SpecRepository : BaseRepository
    {
        public SpecRepository(string connectionString) : base(connectionString) { }

        public List<Spec> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<Spec>("SELECT * FROM Spec ORDER BY Id").ToList();
        }

        public Spec GetByDisplaySpec(string displaySpec)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Spec>(
                    "SELECT * FROM Spec WHERE DisplaySpec = @DisplaySpec",
                    new { DisplaySpec = displaySpec });
        }

        public void Add(Spec spec)
        {
            using (var conn = CreateConnection())
                conn.Execute(@"INSERT INTO Spec (DisplaySpec, Comp1, Comp2, Comp3, ConvertedSpec, CalcBase) 
                               VALUES (@DisplaySpec, @Comp1, @Comp2, @Comp3, @ConvertedSpec, @CalcBase)", spec);
        }

        public Spec GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Spec>("SELECT * FROM Spec WHERE Id = @Id", new { Id = id });
        }

        public void Update(Spec spec, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute(@"UPDATE Spec SET DisplaySpec = @DisplaySpec, Comp1 = @Comp1, Comp2 = @Comp2, 
                               Comp3 = @Comp3, ConvertedSpec = @ConvertedSpec, CalcBase = @CalcBase 
                               WHERE Id = @Id", new { spec.DisplaySpec, spec.Comp1, spec.Comp2, spec.Comp3, spec.ConvertedSpec, spec.CalcBase, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM Spec WHERE Id = @Id", new { Id = id });
        }

        // 检查DisplaySpec是否存在，排除指定Id的记录，即显示规格是否唯一
        public bool Exists(string displaySpec, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM Spec WHERE DisplaySpec = @DisplaySpec";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { DisplaySpec = displaySpec, ExcludeId = excludeId });
                return count > 0;
            }
        }

        // 检查三组参与计算的数值是否存在重复，第三组可能为空
        public bool ExistsByComponents(string comp1, string comp2, string comp3, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                // 第三组可能为空，需要特殊处理
                string sql;
                if (string.IsNullOrWhiteSpace(comp3))
                {
                    sql = @"SELECT COUNT(1) FROM Spec 
                    WHERE Comp1 = @Comp1 AND Comp2 = @Comp2 
                      AND (Comp3 IS NULL OR Comp3 = '')";
                }
                else
                {
                    sql = @"SELECT COUNT(1) FROM Spec 
                    WHERE Comp1 = @Comp1 AND Comp2 = @Comp2 AND Comp3 = @Comp3";
                }

                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";

                var count = conn.ExecuteScalar<int>(sql, new
                {
                    Comp1 = comp1,
                    Comp2 = comp2,
                    Comp3 = string.IsNullOrWhiteSpace(comp3) ? null : comp3,
                    ExcludeId = excludeId
                });
                return count > 0;
            }
        }
    }
}
