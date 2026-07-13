using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 涂层DAO
    public class CoatingRepository : BaseRepository
    {
        public CoatingRepository(string connectionString) : base(connectionString) { }

        // 根据品牌ID获取该品牌下的涂层列表
        public List<Coating> GetByBrandId(int brandId)
        {
            using (var conn = CreateConnection())
                return conn.Query<Coating>(
                    "SELECT * FROM Coating WHERE BrandId = @BrandId ORDER BY Id",
                    new { BrandId = brandId }).ToList();
        }

        public Coating GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Coating>("SELECT * FROM Coating WHERE Id = @Id", new { Id = id });
        }

        public void Add(Coating coating)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO Coating (BrandId, Name) VALUES (@BrandId, @Name)", coating);
        }

        public List<Coating> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<Coating>("SELECT * FROM Coating ORDER BY Id").ToList();
        }

        /// <summary>
        /// 根据品牌ID删除该品牌下的所有涂层
        /// </summary>
        /// <param name="brandId"></param>
        public void DeleteByBrandId(int brandId)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM Coating WHERE BrandId = @BrandId", new { BrandId = brandId });
        }

        /// <summary>
        /// 根据涂层名称获取涂层信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Coating GetByName(string name)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Coating>("SELECT * FROM Coating WHERE Name = @Name", new { Name = name });
        }

        /// <summary>
        /// 更新涂层信息
        /// </summary>
        /// <param name="coating"></param>
        public void Update(Coating coating)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE Coating SET BrandId = @BrandId, Name = @Name WHERE Id = @Id", coating);
        }

        public void Update(Coating coating, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE Coating SET BrandId = @BrandId, Name = @Name WHERE Id = @Id", new { coating.BrandId, coating.Name, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM Coating WHERE Id = @Id", new { Id = id });
        }

        public List<CoatingViewModel> GetAllWithBrand()
        {
            using (var conn = CreateConnection())
            {
                string sql = @"
            SELECT c.Id, c.BrandId, c.Name, b.DisplayName AS BrandName
            FROM Coating c
            JOIN Brand b ON c.BrandId = b.Id
            ORDER BY c.Id";
                return conn.Query<CoatingViewModel>(sql).ToList();
            }
        }

        public bool Exists(int brandId, string name, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM Coating WHERE BrandId = @BrandId AND Name = @Name";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { BrandId = brandId, Name = name, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
