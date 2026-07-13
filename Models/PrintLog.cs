using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 打印日志表
    public class PrintLog
    {
        public int Id { get; set; }
        public int BarcodeId { get; set; }
        public string PrintedTime { get; set; }   // 打印时间（SQLite存TEXT）
        public bool IsSuccess { get; set; }       // 是否打印成功
        public string Remark { get; set; }        // 备注（补打/作废等）

        // 导航属性
        public Barcode Barcode { get; set; }

        public override string ToString()
        {
            return $"PrintLog: BarcodeId={BarcodeId}, PrintedTime={PrintedTime}, Success={IsSuccess}";
        }
    }
}
