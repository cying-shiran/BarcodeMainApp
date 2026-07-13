using BarcodeMainApp.DataAccess;
using BarcodeMainApp.Models;
using BarcodeMainApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace BarcodeMainApp.Forms
{
    public partial class DataEditForm : Form
    {
        private string _tableName;
        private BrandRepository _brandRepo;
        // 编辑时原实体，新增时为null
        private object _originalEntity; 
        // 存储 ComboBox 控件（如果字段需要下拉）
        private Dictionary<string, ComboBox> _comboBoxes = new Dictionary<string, ComboBox>();
        private Dictionary<string, TextBox> _textBoxes = new Dictionary<string, TextBox>();
        private Dictionary<string, string[]> _tableFields = new Dictionary<string, string[]>
{
    { "Brand", new[] { "DisplayName", "Code" } },
    { "Coating", new[] { "BrandId", "Name" } }, // BrandId 可后续改为下拉框，先用文本框
    { "Category", new[] { "Name" } },
    { "ToolName", new[] { "Name" } },
    { "ProductCode", new[] { "DisplayCode", "ConvertedCode" } },
    { "Spec", new[] { "DisplaySpec", "Comp1", "Comp2", "Comp3", "ConvertedSpec", "CalcBase" } }
};

        private Dictionary<string, string> _fieldLabels = new Dictionary<string, string>
{
    { "DisplayName", "显示名称" },
    { "Code", "编码 (1位)" },
    { "BrandId", "品牌ID" },
    { "Name", "名称" },
    { "DisplayCode", "显示编号" },
    { "ConvertedCode", "转换后编号" },
    { "DisplaySpec", "显示规格" },
    { "Comp1", "组分1" },
    { "Comp2", "组分2" },
    { "Comp3", "组分3" },
    { "ConvertedSpec", "转换后规格" },
    { "CalcBase", "计算基数" }
};

        public DataEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当窗体加载时触发的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataEditForm_Load(object sender, EventArgs e)
        {

        }

        // 返回用户确认后的实体对象（调用方根据表名自行转换）
        public object ResultEntity { get; private set; }

        /// <summary>
        /// tableName: 表名
        //  entity: 若为编辑，传入原始对象；新增则为null
        //  brandRepo: 用于获取品牌列表
        //  connectionString: 用于检查重复记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="brandRepo"></param>
        /// <param name="connectionString"></param>
        /// <param name="entity"></param>
        public DataEditForm(string tableName, BrandRepository brandRepo, object entity = null)
        {
            InitializeComponent();
            _brandRepo = brandRepo;
            _tableName = tableName;
            _originalEntity = entity;
            this.Text = entity == null ? $"新增 - {GetTableDisplayName(tableName)}" : $"编辑 - {GetTableDisplayName(tableName)}";
            GenerateFields();
            if (entity != null)
                BindEntityToFields(entity);
        }

        /// <summary>
        /// 根据表名获取对应的显示名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetTableDisplayName(string tableName)
        {
            switch (tableName)
            {
                case "Brand": return "品牌";
                case "Coating": return "涂层";
                case "Category": return "刀具类别";
                case "ToolName": return "刀具名称";
                case "ProductCode": return "编号";
                case "Spec": return "规格";
                default: return tableName;
            }
        }

        /// <summary>
        /// 根据当前表名生成对应的字段输入控件
        /// </summary>
        private void GenerateFields()
        {

            if (!_tableFields.ContainsKey(_tableName)) return;

            string[] fields = _tableFields[_tableName];
            tblFields.RowCount = fields.Length;
            tblFields.RowStyles.Clear();
            for (int i = 0; i < fields.Length; i++)
                tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i];
                Label lbl = new Label { Text = _fieldLabels.ContainsKey(field) ? _fieldLabels[field] : field, AutoSize = false, Width = 90, TextAlign = ContentAlignment.MiddleRight };
                tblFields.Controls.Add(lbl, 0, i);

                // 如果是 BrandId，使用 ComboBox
                if (field == "BrandId" && _tableName == "Coating")
                {
                    ComboBox cmb = new ComboBox { Name = $"cmb{field}", Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                    var brands = _brandRepo.GetAll();
                    cmb.DataSource = brands;
                    cmb.DisplayMember = "DisplayName";
                    cmb.ValueMember = "Id";
                    tblFields.Controls.Add(cmb, 1, i);
                    _comboBoxes[field] = cmb;
                } 
                else if (field == "ConvertedCode" && _tableName == "ProductCode") // 在 GenerateFields 中，针对 ProductCode 的 ConvertedCode 字段
                {
                    TextBox txt = new TextBox { Name = $"txt{field}", Width = 150, ReadOnly = true, BackColor = SystemColors.Control };
                    tblFields.Controls.Add(txt, 1, i);
                    _textBoxes[field] = txt;
                }
                else if (field == "ConvertedSpec" && _tableName == "Spec") // 在 GenerateFields 中，针对 Spec 的 ConvertedSpec 字段
                {
                    TextBox txt = new TextBox { Name = $"txt{field}", Width = 150, ReadOnly = true, BackColor = SystemColors.Control };
                    tblFields.Controls.Add(txt, 1, i);
                    _textBoxes[field] = txt;
                }
                else if ((field == "DisplaySpec" && _tableName == "Spec"))
                {
                    TextBox txt = new TextBox { Name = $"txt{field}", Width = 150 };
                    tblFields.Controls.Add(txt, 1, i);
                    _textBoxes[field] = txt;
                    txt.TextChanged += TxtDisplaySpec_TextChanged;
                }
                else
                {
                    TextBox txt = new TextBox { Name = $"txt{field}", Width = 150 };
                    if (field == "CalcBase") txt.Text = "16";

                    // 绑定编号输入事件，自动填充转换编号
                    if (field == "DisplayCode")
                        txt.TextChanged += TxtDisplayCode_TextChanged;

                    if (_tableName == "Spec" && (field == "Comp1" || field == "Comp2" || field == "Comp3" || field == "CalcBase"))
                    {
                        txt.TextChanged += SpecField_TextChanged;
                    }

                    // 其他字段的处理
                    tblFields.Controls.Add(txt, 1, i);
                    _textBoxes[field] = txt;
                }

            }
        }

        /// <summary>
        /// 分割显示规格DisplaySpec字段，并自动填充 Comp1、Comp2、Comp3 文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDisplaySpec_TextChanged(object sender, EventArgs e)
        {
            if (_tableName != "Spec") return;
            string display = _textBoxes["DisplaySpec"].Text;
            if (string.IsNullOrWhiteSpace(display))
            {
                _textBoxes["Comp1"].Text = "";
                _textBoxes["Comp2"].Text = "";
                _textBoxes["Comp3"].Text = "";
                return;
            }

            // 第一步：按 × 分割
            var parts = display.Split(new[] { '×', 'x', 'X' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim()).ToList();

            // 第二步：对每个部分尝试智能拆分（字母+数字 → 字母 + 数字）
            var expandedParts = new List<string>();
            foreach (var part in parts)
            {
                // 匹配类似 R1.5, T007, R2 等
                // 1. 字母+数字（如 R1.5）
                var matchLetterNum = Regex.Match(part, @"^([A-Za-z]+)(\d.*)$");
                if (matchLetterNum.Success)
                {
                    expandedParts.Add(matchLetterNum.Groups[1].Value);
                    expandedParts.Add(matchLetterNum.Groups[2].Value);
                    continue;
                }

                // 2. 数字部分/字母部分（如 70mm/R）
                var matchNumLetter = Regex.Match(part, @"^(\d+(?:\.\d+)?.*?)/([A-Za-z]+)$");
                if (matchNumLetter.Success)
                {
                    expandedParts.Add(matchNumLetter.Groups[1].Value);
                    expandedParts.Add(matchNumLetter.Groups[2].Value);
                    continue;
                }

                expandedParts.Add(part);
            }

            // 第三步：清理每个部分（去单位等）
            for (int i = 0; i < expandedParts.Count; i++)
            {
                expandedParts[i] = CleanSpecPart(expandedParts[i]);
            }

            // 第四步：填充到三个文本框
            if (expandedParts.Count > 0) _textBoxes["Comp1"].Text = expandedParts[0];
            if (expandedParts.Count > 1) _textBoxes["Comp2"].Text = expandedParts[1];
            if (expandedParts.Count > 2) _textBoxes["Comp3"].Text = expandedParts[2];
            if (expandedParts.Count < 3) _textBoxes["Comp3"].Text = "";
            if (expandedParts.Count < 2) _textBoxes["Comp2"].Text = "";
        }

        /// <summary>
        /// 清理规格部分，去除单位和多余符号
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private string CleanSpecPart(string part)
        {
            if (string.IsNullOrEmpty(part)) return part;
            // 保留 ° 符号，去除 mm、cm 等单位
            part = Regex.Replace(part, @"\s*mm\s*$", "", RegexOptions.IgnoreCase);
            part = Regex.Replace(part, @"\s*cm\s*$", "", RegexOptions.IgnoreCase);
            // 去掉尾部多余的 / 或空格
            part = part.TrimEnd('/', ' ');
            return part;
        }

        /// <summary>
        /// 自动填充 ConvertedCode 转换编号字段，当用户输入 DisplayCode 编号字段时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDisplayCode_TextChanged(object sender, EventArgs e)
        {
            if (_tableName != "ProductCode") return;
            var displayCode = _textBoxes["DisplayCode"].Text;
            if (string.IsNullOrWhiteSpace(displayCode))
            {
                _textBoxes["ConvertedCode"].Text = "";
                return;
            }
            try
            {
                string converted = SpecChange.ConvertToCode(displayCode);
                _textBoxes["ConvertedCode"].Text = converted;
            }
            catch (Exception ex)
            {
                _textBoxes["ConvertedCode"].Text = "错误：" + ex.Message;
            }
        }

        private void SpecField_TextChanged(object sender, EventArgs e)
        {
            if (_tableName != "Spec") return;
            CalculateAndShowSpec();
        }

        /// <summary>
        /// 绑定 Spec 相关字段的输入事件，当用户输入 Comp1、Comp2、Comp3 或 CalcBase 时，自动计算并显示转换后的规格
        /// </summary>
        private void CalculateAndShowSpec()
        {
            try
            {
                string comp1 = _textBoxes["Comp1"].Text;
                string comp2 = _textBoxes["Comp2"].Text;
                string comp3 = _textBoxes.ContainsKey("Comp3") ? _textBoxes["Comp3"].Text : "";
                string calcBaseText = _textBoxes["CalcBase"].Text;

                if (string.IsNullOrWhiteSpace(comp1) || string.IsNullOrWhiteSpace(comp2) || string.IsNullOrWhiteSpace(calcBaseText))
                {
                    _textBoxes["ConvertedSpec"].Text = "";
                    return;
                }

                if (!double.TryParse(calcBaseText, out double calcBase) || calcBase <= 0)
                {
                    _textBoxes["ConvertedSpec"].Text = "基数无效";
                    return;
                }

                // 假设已有 SpecCalculator 工具类
                string c1 = SpecChange.CalculateSpecComponent(comp1, calcBase);
                string c2 = SpecChange.CalculateSpecComponent(comp2, calcBase);
                string c3 = SpecChange.CalculateSpecComponent(comp3, calcBase);

                _textBoxes["ConvertedSpec"].Text = c1 + c2 + c3;
            }
            catch (Exception ex)
            {
                _textBoxes["ConvertedSpec"].Text = "错误：" + ex.Message;
            }
        }

        /// <summary>
        /// 将实体对象的属性值绑定到对应的文本框中
        /// </summary>
        /// <param name="entity"></param>
        private void BindEntityToFields(object entity)
        {
            var type = entity.GetType();
            // 文本框
            foreach (var field in _textBoxes.Keys)
            {
                var prop = type.GetProperty(field);
                if (prop != null)
                    _textBoxes[field].Text = prop.GetValue(entity)?.ToString() ?? "";
            }
            // 下拉框
            foreach (var field in _comboBoxes.Keys)
            {
                var prop = type.GetProperty(field);
                if (prop != null && _comboBoxes.ContainsKey(field))
                    _comboBoxes[field].SelectedValue = prop.GetValue(entity);
            }
        }

        /// <summary>
        /// 当用户点击“确定”按钮时触发的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = CreateEntityFromFields();
                ResultEntity = entity;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输入数据有误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据当前表名和文本框内容创建对应的实体对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private object CreateEntityFromFields()
        {
            switch (_tableName)
            {
                case "Brand":
                    return new Brand
                    {
                        DisplayName = _textBoxes["DisplayName"].Text,
                        Code = _textBoxes["Code"].Text
                    };
                case "Coating":
                    return new Coating
                    {
                        BrandId = (int)_comboBoxes["BrandId"].SelectedValue,
                        Name = _textBoxes["Name"].Text
                    };
                case "Category":
                    return new Category { Name = _textBoxes["Name"].Text };
                case "ToolName":
                    return new ToolName { Name = _textBoxes["Name"].Text };
                case "ProductCode":
                    return new ProductCode
                    {
                        DisplayCode = _textBoxes["DisplayCode"].Text,
                        ConvertedCode = _textBoxes["ConvertedCode"].Text   // 自动计算好的
                    };
                case "Spec":
                    var spec = new Spec
                    {
                        DisplaySpec = _textBoxes["DisplaySpec"].Text,
                        Comp1 = _textBoxes["Comp1"].Text,
                        Comp2 = _textBoxes["Comp2"].Text,
                        Comp3 = _textBoxes["Comp3"].Text,
                        CalcBase = int.Parse(_textBoxes["CalcBase"].Text),
                        ConvertedSpec = _textBoxes["ConvertedSpec"].Text  // 已自动计算
                    };
                    //spec.ConvertedSpec = SpecChange.ConvertSpec(spec);
                    return spec;
                default: throw new Exception("不支持的表名");
            }
        }

        

        /// <summary>
        /// 当用户点击“取消”按钮时触发的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; 
            Close();
        }
    }
}
