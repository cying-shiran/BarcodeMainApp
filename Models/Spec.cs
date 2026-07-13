using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 规格表
    public class Spec
    {
        public int Id { get; set; }
        public string DisplaySpec { get; set; }   // 显示规格，如 "1/2×6×20mm"
        public string Comp1 { get; set; }         // 计算组分1
        public string Comp2 { get; set; }         // 计算组分2
        public string Comp3 { get; set; }         // 计算组分3
        public string ConvertedSpec { get; set; } // 转换后6位，如 "080620"
        public int CalcBase { get; set; } = 16;   // 计算进制

        public override string ToString()
        {
            return $"{DisplaySpec} → {ConvertedSpec} (Base={CalcBase})";
        }
    }
}
