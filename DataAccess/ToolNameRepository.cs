using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 刀具名称DAO
    public class ToolNameRepository : BaseRepository
    {
        public ToolNameRepository(string connectionString) : base(connectionString) { }

        public List<ToolName> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<ToolName>("SELECT * FROM ToolName ORDER BY Id").ToList();
        }

        public ToolName GetById(int id)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<ToolName>("SELECT * FROM ToolName WHERE Id = @Id", new { Id = id });
        }

        public void Add(ToolName toolName)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO ToolName (Name) VALUES (@Name)", toolName);
        }

        public void Update(ToolName toolName, int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("UPDATE ToolName SET Name = @Name WHERE Id = @Id", new { toolName.Name, Id = id });
        }

        public void Delete(int id)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM ToolName WHERE Id = @Id", new { Id = id });
        }

        public bool Exists(string name, int? excludeId = null)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT COUNT(1) FROM ToolName WHERE Name = @Name";
                if (excludeId.HasValue)
                    sql += " AND Id != @ExcludeId";
                var count = conn.ExecuteScalar<int>(sql, new { Name = name, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
