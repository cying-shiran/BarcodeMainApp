using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    public class CategoryToolRelationRepository : BaseRepository
    {
        public CategoryToolRelationRepository(string cs) : base(cs) { }

        // 根据类别获取刀具名称列表
        public List<ToolName> GetToolNamesByCategory(int categoryId)
        {
            using (var conn = CreateConnection())
                return conn.Query<ToolName>(
                    @"SELECT DISTINCT t.* FROM CategoryToolRelation r
                      JOIN ToolName t ON r.ToolNameId = t.Id
                      WHERE r.CategoryId = @CategoryId",
                    new { CategoryId = categoryId }).ToList();
        }

        // 根据类别+刀具名称获取可选编号
        public List<ProductCode> GetCodesByCategoryAndTool(int categoryId, int toolNameId)
        {
            using (var conn = CreateConnection())
                return conn.Query<ProductCode>(
                    @"SELECT DISTINCT pc.* FROM CategoryToolRelation r
                      JOIN ProductCode pc ON r.ProductCodeId = pc.Id
                      WHERE r.CategoryId = @CategoryId AND r.ToolNameId = @ToolNameId",
                    new { CategoryId = categoryId, ToolNameId = toolNameId }).ToList();
        }

        // 根据类别+刀具名称+编号获取可选规格
        public List<Spec> GetSpecsByCategoryToolAndCode(int categoryId, int toolNameId, int productCodeId)
        {
            using (var conn = CreateConnection())
                return conn.Query<Spec>(
                    @"SELECT DISTINCT s.* FROM CategoryToolRelation r
                      JOIN Spec s ON r.SpecId = s.Id
                      WHERE r.CategoryId = @CategoryId 
                        AND r.ToolNameId = @ToolNameId 
                        AND r.ProductCodeId = @ProductCodeId",
                    new { CategoryId = categoryId, ToolNameId = toolNameId, ProductCodeId = productCodeId }).ToList();
        }

        // 添加新的关系
        public void Add(int categoryId, int toolNameId, int productCodeId, int specId)
        {
            using (var conn = CreateConnection())
                conn.Execute("INSERT INTO CategoryToolRelation (CategoryId, ToolNameId, ProductCodeId, SpecId) VALUES (@cat, @tool, @code, @spec)",
                    new { cat = categoryId, tool = toolNameId, code = productCodeId, spec = specId });
        }

        // 删除关系
        public void Delete(int categoryId, int toolNameId, int productCodeId, int specId)
        {
            using (var conn = CreateConnection())
                conn.Execute("DELETE FROM CategoryToolRelation WHERE CategoryId=@cat AND ToolNameId=@tool AND ProductCodeId=@code AND SpecId=@spec",
                    new { cat = categoryId, tool = toolNameId, code = productCodeId, spec = specId });
        }

      
    }
}
