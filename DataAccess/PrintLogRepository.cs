using BarcodeMainApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.DataAccess
{
    // 打印日志DAO
    public class PrintLogRepository : BaseRepository
    {
        public PrintLogRepository(string connectionString) : base(connectionString) { }

        // 根据条码ID查询打印历史
        public List<PrintLog> GetByBarcodeId(int barcodeId)
        {
            using (var conn = CreateConnection())
                return conn.Query<PrintLog>(
                    "SELECT * FROM PrintLog WHERE BarcodeId = @BarcodeId ORDER BY PrintedTime DESC",
                    new { BarcodeId = barcodeId }).ToList();
        }

        // 新增打印记录
        public void Add(PrintLog log)
        {
            using (var conn = CreateConnection())
                conn.Execute(@"INSERT INTO PrintLog (BarcodeId, PrintedTime, IsSuccess, Remark) 
                               VALUES (@BarcodeId, @PrintedTime, @IsSuccess, @Remark)", log);
        }
    }
}
