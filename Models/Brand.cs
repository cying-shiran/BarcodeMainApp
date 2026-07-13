using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 品牌模型
    public class Brand
    {
        // 品牌ID
        public int Id { get; set; }
        // 品牌显示名称
        public string DisplayName { get; set; }
        public string Code { get; set; }           // 1位字符，区分大小写，如 "1", "A", "b"

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
