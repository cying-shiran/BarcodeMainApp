using BarcodeMainApp.DataAccess;
using BarcodeMainApp.Forms;
using BarcodeMainApp.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BarcodeMainApp
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // 加载数据的方法，连接到SQLite数据库并验证连接是否成功。
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BarcodeApp.db");
            var connectionString = $"Data Source={dbPath}";
            DatabaseInitializer.Initialize(connectionString);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 启动主窗体
            var mainForm = new BarcodeEntryForm(connectionString);
            ThemeHelper.Apply(mainForm);
            Application.Run(mainForm);
        }

        private static void spec()
        {
            //// 规格 "1/2×7/8×3/8" → 三个组分
            //string comp1 = SpecChange.CalculateSpecComponent("1/4");   // 返回 8
            //string comp2 = SpecChange.CalculateSpecComponent("5/16");   // 返回 14
            //string comp3 = SpecChange.CalculateSpecComponent("1.5");   // 返回 6
            //string specCode = $"{comp1:D2}{comp2:D2}{comp3:D2}"; // "081406"
            //Debug.WriteLine($"解析结果：{specCode}"); // 输出：解析结果
        }
    }
}
