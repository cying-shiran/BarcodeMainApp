using BarcodeMainApp.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeMainApp
{
    public partial class MainForm : Form
    {
        private string _connectionString;
        private CategoryRepository _categoryRepo;
        private BrandRepository _brandRepo;
        private CategoryToolRelationRepository _catToolRepo;
        private ToolNameRepository _toolNameRepo;
        private CoatingRepository _coatingRepo;
        private ProductCodeRepository _productCodeRepo;
        private SpecRepository _specRepo;
        private BarcodeRepository _barcodeRepo;
        private PrintLogRepository _printLogRepo;

        public MainForm()
        {
            InitializeComponent();

            // 设置数据库路径和连接字符串
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BarcodeApp.db");
            _connectionString = $"Data Source={dbPath}";

            // 初始化所有 Repository
            _categoryRepo = new CategoryRepository(_connectionString);
            _brandRepo = new BrandRepository(_connectionString);
            _catToolRepo = new CategoryToolRelationRepository(_connectionString);
            _toolNameRepo = new ToolNameRepository(_connectionString);
            _coatingRepo = new CoatingRepository(_connectionString);
            _productCodeRepo = new ProductCodeRepository(_connectionString);
            _specRepo = new SpecRepository(_connectionString);
            _barcodeRepo = new BarcodeRepository(_connectionString);
            _printLogRepo = new PrintLogRepository(_connectionString);
        }
    }
}
