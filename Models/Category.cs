using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 类别模型
    public class Category
    {
        //  类别ID
        public int Id { get; set; }
        // 类别名称
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
