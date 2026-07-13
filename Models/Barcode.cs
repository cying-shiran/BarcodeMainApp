using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 条码信息表
    public class Barcode
    {
        public int Id { get; set; }
        public string RouteCode { get; set; }     // 7位路线单号
        public int CategoryId { get; set; } // 外键，关联到 Category 表
        public int BrandId { get; set; } // 外键，关联到 Brand 表
        public int ToolNameId { get; set; } // 外键，关联到 ToolName 表
        public int CoatingId { get; set; } // 外键，关联到 Coating 表
        public int ProductCodeId { get; set; } // 外键，关联到 ProductCode 表
        public int SpecId { get; set; } // 外键，关联到 Spec 表
        public int Quantity { get; set; }         // 原始数量
        public string FullBarcode { get; set; }   // 30位完整条码
        public string OrderDate { get; set; }     // 业务日期（SQLite存TEXT）
        public string CreatedTime { get; set; }   // 录入时间
        public bool IsPrinted { get; set; } // 是否已打印
        public bool IsTransmitted { get; set; } // 是否已传输

        // 导航属性
        public Category Category { get; set; } //   导航属性，关联到 Category 实体
        public Brand Brand { get; set; } // 导航属性，关联到 Brand 实体
        public ToolName ToolName { get; set; } // 导航属性，关联到 ToolName 实体
        public Coating Coating { get; set; } // 导航属性，关联到 Coating 实体
        public ProductCode ProductCode { get; set; } // 导航属性，关联到 ProductCode 实体
        public Spec Spec { get; set; } // 导航属性，关联到 Spec 实体
        public ICollection<PrintLog> PrintLogs { get; set; } // 一对多

        public override string ToString()
        {
            return $"Route={RouteCode}, Full={FullBarcode}, Qty={Quantity}";
        }
    }
}
