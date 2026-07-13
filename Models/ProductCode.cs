using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 产品编号映射表
    public class ProductCode
    {
        public int Id { get; set; }
        public string DisplayCode { get; set; }   // 原始编号，如 "T007-W"
        public string ConvertedCode { get; set; } // 转换后7位，如 "2000723"

        public override string ToString()
        {
            return $"{DisplayCode} → {ConvertedCode}";
        }
    }
}
