using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    //  类别DAO
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(string connectionString) : base(connectionString) { }

        public List<Category> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<Category>("SELECT * FROM Category ORDER BY Id").ToList();
        }

        public Category GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Category>("SELECT * FROM Category WHERE Id = @Id", new { Id = id });
        }

        public void Add(Category category)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO Category (Name) VALUES (@Name)", category);
        }

        public void Update(Category category, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE Category SET Name = @Name WHERE Id = @Id", new { category.Name, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM Category WHERE Id = @Id", new { Id = id });
        }

        public bool Exists(string name, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM Category WHERE Name = @Name";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { Name = name, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
