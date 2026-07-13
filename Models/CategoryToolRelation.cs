using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 刀具类别与刀具名称的关系
    public class CategoryToolRelation
    {
        public int Id { get; set; }
        public int CategoryId { get; set; } // 外键，关联到 Category 表
        public int ToolNameId { get; set; } // 外键，关联到 ToolName 表
        public int ProductCodeId { get; set; } // 外键，关联到 ProductCode 表
        public int SpecId { get; set; } // 外键，关联到 Spec 表

        // 导航属性
        public Category Category { get; set; } // 导航属性，关联到 Category 实体
        public ToolName ToolName { get; set; } // 导航属性，关联到 ToolName 实体
        public ProductCode ProductCode { get; set; } //     导航属性，关联到 ProductCode 实体
        public Spec Spec { get; set; } // 导航属性，关联到 Spec 实体

        public override string ToString()
            => $"Category={Category?.Name}, Tool={ToolName?.Name}, Code={ProductCode?.DisplayCode}";
    }
}
