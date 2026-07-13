using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeMainApp.Models
{
    // 涂层
    public class Coating
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }

        // 导航属性
        public Brand Brand { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
