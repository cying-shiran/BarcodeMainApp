using BarcodeMainApp.Models;
using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 条码DAO
    public class BarcodeRepository : BaseRepository
    {
        public BarcodeRepository(string connectionString) : base(connectionString) { }

        // 按路线单号查询
        public List<Barcode> GetByRouteCode(string routeCode)
        {
            using (var conn = CreateConnection())
                return conn.Query<Barcode>(
                    "SELECT * FROM Barcode WHERE RouteCode = @RouteCode ORDER BY Id",
                    new { RouteCode = routeCode }).ToList();
        }

        // 按日期范围查询
        public List<Barcode> GetByDateRange(DateTime start, DateTime end)
        {
            using (var conn = CreateConnection())
                return conn.Query<Barcode>(
                    "SELECT * FROM Barcode WHERE OrderDate BETWEEN @Start AND @End ORDER BY OrderDate",
                    new { Start = start.ToString("yyyy-MM-dd"), End = end.ToString("yyyy-MM-dd") }).ToList();
        }

        // 新增条码（返回自增ID）
        public int Add(Barcode barcode)
        {
            using (var conn = CreateConnection())
            {
                conn.Execute(@"INSERT INTO Barcode (RouteCode, CategoryId, BrandId, ToolNameId, CoatingId, 
                                ProductCodeId, SpecId, Quantity, FullBarcode, OrderDate, CreatedTime) 
                                VALUES (@RouteCode, @CategoryId, @BrandId, @ToolNameId, @CoatingId, 
                                @ProductCodeId, @SpecId, @Quantity, @FullBarcode, @OrderDate, @CreatedTime)", barcode);
                // 获取最后插入的行ID
                return conn.QuerySingle<int>("SELECT last_insert_rowid()");
            }
        }

        // 根据ID获取条码信息
        public Barcode GetById(int barcodeId)
        {
            using (var conn = CreateConnection())
                return conn.QuerySingleOrDefault<Barcode>(
                    "SELECT * FROM Barcode WHERE Id = @Id",
                    new { Id = barcodeId });
        }

        // 更新条码信息
        public void Update(Barcode barcode)
        {
            if (barcode == null) return;
            using (var conn = CreateConnection())
            {
                conn.Execute(@"UPDATE Barcode SET RouteCode = @RouteCode, CategoryId = @CategoryId, 
                                BrandId = @BrandId, ToolNameId = @ToolNameId, CoatingId = @CoatingId, 
                                ProductCodeId = @ProductCodeId, SpecId = @SpecId, Quantity = @Quantity, 
                                FullBarcode = @FullBarcode, OrderDate = @OrderDate, CreatedTime = @CreatedTime 
                                WHERE Id = @Id", barcode);
            }
        }

        // 删除条码信息
        public void Delete(int barcodeId)
        {
            using (var conn = CreateConnection())
            {
                conn.Execute("DELETE FROM Barcode WHERE Id = @Id", new { Id = barcodeId });
            }
        }

        // 按日期查询
        public List<Barcode> GetByDate(string today)
        {
            using (var conn = CreateConnection())
                return conn.Query<Barcode>(
                    "SELECT * FROM Barcode WHERE OrderDate = @Today ORDER BY Id",
                    new { Today = today }).ToList();
        }

        // 获取所有条码信息
        public List<Barcode> GetAll()
        {
            using (var conn = CreateConnection())
                return conn.Query<Barcode>(
                    "SELECT * FROM Barcode ORDER BY Id").ToList();
        }

        // 设置条码的传输状态
        public void SetTransmitted(int barcodeId, bool isTransmitted)
        {
            using (var conn = CreateConnection())
            {
                conn.Execute("UPDATE Barcode SET IsTransmitted = @IsTransmitted WHERE Id = @Id",
                    new { Id = barcodeId, IsTransmitted = isTransmitted ? 1 : 0 });
            }
        }

        public List<Barcode> Search(string routeCode = null, DateTime? startDate = null, DateTime? endDate = null,
    int? brandId = null, int? categoryId = null, int? toolNameId = null, int? productCodeId = null,
    int? specId = null, bool? isPrinted = null, bool? isTransmitted = null)
        {
            using (var conn = CreateConnection())
            {
                var sql = "SELECT * FROM Barcode WHERE 1=1 ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(routeCode))
                {
                    sql += " AND RouteCode LIKE @RouteCode ";
                    parameters.Add("RouteCode", $"%{routeCode}%");
                }
                if (startDate.HasValue)
                {
                    sql += " AND OrderDate >= @StartDate ";
                    parameters.Add("StartDate", startDate.Value.ToString("yyyy-MM-dd"));
                }
                if (endDate.HasValue)
                {
                    sql += " AND OrderDate <= @EndDate ";
                    parameters.Add("EndDate", endDate.Value.ToString("yyyy-MM-dd"));
                }
                if (brandId.HasValue && brandId.Value > 0)
                {
                    sql += " AND BrandId = @BrandId ";
                    parameters.Add("BrandId", brandId.Value);
                }
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    sql += " AND CategoryId = @CategoryId ";
                    parameters.Add("CategoryId", categoryId.Value);
                }
                if (toolNameId.HasValue && toolNameId.Value > 0)
                {
                    sql += " AND ToolNameId = @ToolNameId ";
                    parameters.Add("ToolNameId", toolNameId.Value);
                }
                if (productCodeId.HasValue && productCodeId.Value > 0)
                {
                    sql += " AND ProductCodeId = @ProductCodeId ";
                    parameters.Add("ProductCodeId", productCodeId.Value);
                }
                if (specId.HasValue && specId.Value > 0)
                {
                    sql += " AND SpecId = @SpecId ";
                    parameters.Add("SpecId", specId.Value);
                }
                if (isPrinted.HasValue)
                {
                    sql += " AND IsPrinted = @IsPrinted ";
                    parameters.Add("IsPrinted", isPrinted.Value ? 1 : 0);
                }
                if (isTransmitted.HasValue)
                {
                    sql += " AND IsTransmitted = @IsTransmitted ";
                    parameters.Add("IsTransmitted", isTransmitted.Value ? 1 : 0);
                }

                sql += " ORDER BY OrderDate DESC, Id DESC LIMIT 500";

                return conn.Query<Barcode>(sql, parameters).ToList();
            }
        }
    }
}
