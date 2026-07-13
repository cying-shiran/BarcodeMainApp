using BarcodeMainApp.DataAccess;
using BarcodeMainApp.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeMainApp
{
    public partial class SearchForm : Form
    {
        private string _connStr;
        // 各类仓库
        private BrandRepository _brandRepo;
        private CoatingRepository _coatingRepo;
        private CategoryRepository _categoryRepo;
        private ToolNameRepository _toolNameRepo;
        private ProductCodeRepository _productCodeRepo;
        private SpecRepository _specRepo;
        private CategoryToolRelationRepository _relationRepo;
        private BarcodeRepository _barcodeRepo;
        // 在 SearchForm 类内部顶部定义一个颜色变量
        private readonly System.Drawing.Color _defaultForeColor = SystemColors.WindowText; // 系统默认文本色
        private bool _isLoading = true;

        public SearchForm(string connectionString)
        {
            InitializeComponent();
            _connStr = connectionString;
            _brandRepo = new BrandRepository(_connStr);
            _coatingRepo = new CoatingRepository(_connStr);
            _categoryRepo = new CategoryRepository(_connStr);
            _toolNameRepo = new ToolNameRepository(_connStr);
            _productCodeRepo = new ProductCodeRepository(_connStr);
            _specRepo = new SpecRepository(_connStr);
            _relationRepo = new CategoryToolRelationRepository(_connStr);
            _barcodeRepo = new BarcodeRepository(_connStr);
            BindEvents();
        }

        /// <summary>
        /// 绑定各个控件的事件处理器，包括Load事件、按钮点击事件等。
        /// </summary>
        private void BindEvents()
        {
            this.Load += SearchForm_Load;
            btnSearch.Click -= btnSearch_Click;
            btnSearch.Click += btnSearch_Click;
            btnClear.Click -= btnClear_Click;
            btnClear.Click += btnClear_Click;
            btnExport.Click -= btnExport_Click;
            btnExport.Click += btnExport_Click;
            btnSend.Click -= btnSend_Click;
            btnSend.Click += btnSend_Click;
            // 级联事件可按需绑定，但查询界面不必强制级联，仅提供选择即可。因此品牌、类别等可以独立加载。
            txtRouteCode.TextChanged += Control_ValueChanged_ResetColor;
            cmbBrand.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            cmbCategory.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            cmbToolName.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            cmbProductCode.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            cmbSpec.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            cmbTransmitStatus.SelectedIndexChanged += Control_ValueChanged_ResetColor;
            dtpStart.ValueChanged += Control_ValueChanged_ResetColor;
            dtpEnd.ValueChanged += Control_ValueChanged_ResetColor;
        }

        /// <summary>
        /// 用户选择条件后，字体加粗显示，便于识别当前筛选条件。此方法用于处理控件的Enter事件，将字体加粗。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_ValueChanged_ResetColor(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (sender is System.Windows.Forms.Control ctrl)
                SetControlBold(ctrl, false);
        }

        /// <summary>
        /// 控件字体加粗辅助方法
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="isBold"></param>
        private void SetControlBold(System.Windows.Forms.Control ctrl, bool isBold)
        {
            if (ctrl == null) return;
            FontStyle style = isBold ? (ctrl.Font.Style | FontStyle.Bold) : (ctrl.Font.Style & ~FontStyle.Bold);
            ctrl.Font = new System.Drawing.Font(ctrl.Font, style);

            // 如果控件是 ComboBox，强制刷新其文本区域
            if (ctrl is ComboBox combo)
            {
                combo.Invalidate();
                combo.Update();
            }
        }

        /// <summary>
        /// 加载查询界面时，初始化各个下拉框的选项，包括品牌、类别、刀具名称、编号、规格等，以及状态下拉框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchForm_Load(object sender, EventArgs e)
        {
            // 品牌
            var brands = _brandRepo.GetAll();
            brands.Insert(0, new Brand { Id = -1, DisplayName = "全部" });
            cmbBrand.DisplayMember = "DisplayName";
            cmbBrand.ValueMember = "Id";
            cmbBrand.DataSource = brands;
            cmbBrand.SelectedIndex = 0;

            // 类别
            var categories = _categoryRepo.GetAll();
            categories.Insert(0, new Models.Category { Id = -1, Name = "全部" });
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            cmbCategory.DataSource = categories;
            cmbCategory.SelectedIndex = 0;

            // 刀具名称等初始为空，可根据需要加载全部（不过滤），也可留空让用户选择类别后加载。为简化，可加载全部刀具名称。
            var tools = _toolNameRepo.GetAll();
            tools.Insert(0, new ToolName { Id = -1, Name = "全部" });
            cmbToolName.DisplayMember = "Name";
            cmbToolName.ValueMember = "Id";
            cmbToolName.DataSource = tools;

            // 编号、规格同理加载全部
            var productCodes = _productCodeRepo.GetAll();
            productCodes.Insert(0, new Models.ProductCode { Id = -1, DisplayCode = "全部" });
            cmbProductCode.DisplayMember = "DisplayCode";
            cmbProductCode.ValueMember = "Id";
            cmbProductCode.DataSource = productCodes;

            var specs = _specRepo.GetAll();
            specs.Insert(0, new Models.Spec { Id = -1, DisplaySpec = "全部" });
            cmbSpec.DisplayMember = "DisplaySpec";
            cmbSpec.ValueMember = "Id";
            cmbSpec.DataSource = specs;

            // 状态下拉
            cmbTransmitStatus.Items.Clear();
            cmbTransmitStatus.Items.AddRange(new[] { "全部", "未传输", "已传输" });
            cmbTransmitStatus.SelectedIndex = 0;

            dtpStart.Checked = false;
            dtpEnd.Checked = false;
            // 初始化数据源...
            _isLoading = false;
        }

        /// <summary>
        /// 查询按钮点击事件，根据用户输入的条件调用BarcodeRepository的Search方法获取结果，并绑定到DataGridView显示。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string routeCode = txtRouteCode.Text.Trim();
            DateTime? start = dtpStart.Checked ? dtpStart.Value.Date : (DateTime?)null;
            DateTime? end = dtpEnd.Checked ? dtpEnd.Value.Date : (DateTime?)null;
            int? brandId = cmbBrand.SelectedIndex > 0 ? (int?)cmbBrand.SelectedValue : null;
            int? categoryId = cmbCategory.SelectedIndex > 0 ? (int?)cmbCategory.SelectedValue : null;
            int? toolNameId = cmbToolName.SelectedIndex > 0 ? (int?)cmbToolName.SelectedValue : null;
            int? productCodeId = cmbProductCode.SelectedIndex > 0 ? (int?)cmbProductCode.SelectedValue : null;
            int? specId = cmbSpec.SelectedIndex > 0 ? (int?)cmbSpec.SelectedValue : null;
            bool? isTransmitted = null;
            if (cmbTransmitStatus.SelectedIndex == 1) isTransmitted = false;
            else if (cmbTransmitStatus.SelectedIndex == 2) isTransmitted = true;

            var results = _barcodeRepo.Search(routeCode, start, end,
                brandId, categoryId, toolNameId, productCodeId, specId,
                isTransmitted);

            dgvResults.DataSource = results.Select(b => new
            {
                路线单号 = b.RouteCode,
                品牌 = _brandRepo.GetById(b.BrandId)?.DisplayName,
                刀具名称 = _toolNameRepo.GetById(b.ToolNameId)?.Name,
                编号 = _productCodeRepo.GetById(b.ProductCodeId)?.DisplayCode,
                规格 = _specRepo.GetById(b.SpecId)?.DisplaySpec,
                数量 = b.Quantity,
                条码 = b.FullBarcode,
                日期 = b.OrderDate,
                //打印状态 = b.IsPrinted ? "已打印" : "未打印",
                传输状态 = b.IsTransmitted ? "已传输" : "未传输",
                BarcodeId = b.Id
            }).ToList();

            if (dgvResults.Columns["BarcodeId"] != null)
                dgvResults.Columns["BarcodeId"].Visible = false;

            // 用当前生效的条件去高亮控件，而非清空条件
            HighlightActiveConditions(routeCode, start, end,
                brandId, categoryId, toolNameId, productCodeId, specId,
                 isTransmitted);

        }

        private void HighlightActiveConditions(string routeCode, DateTime? startDate, DateTime? endDate,
    int? brandId, int? categoryId, int? toolNameId, int? productCodeId, int? specId, bool? isTransmitted)
        {
            SetControlBold(txtRouteCode, !string.IsNullOrEmpty(routeCode));
            SetControlBold(dtpStart, startDate.HasValue);
            SetControlBold(dtpEnd, endDate.HasValue);

            SetControlBold(cmbBrand, cmbBrand.SelectedIndex > 0);
            SetControlBold(cmbCategory, cmbCategory.SelectedIndex > 0);
            SetControlBold(cmbToolName, cmbToolName.SelectedIndex > 0);
            SetControlBold(cmbProductCode, cmbProductCode.SelectedIndex > 0);
            SetControlBold(cmbSpec, cmbSpec.SelectedIndex > 0);
            SetControlBold(cmbTransmitStatus, cmbTransmitStatus.SelectedIndex > 0);

            cmbTransmitStatus.Invalidate();
            cmbTransmitStatus.Update();
        }

        private void SetDtpColor(DateTimePicker dtp, bool isActive)
        {
            dtp.ForeColor = isActive ? System.Drawing.Color.Red : _defaultForeColor;
            dtp.CalendarForeColor = isActive ? System.Drawing.Color.Red : _defaultForeColor;
            // 加粗更明显
            dtp.Font = isActive ? new System.Drawing.Font(dtp.Font, FontStyle.Bold) : new System.Drawing.Font(dtp.Font, FontStyle.Regular);
        }

        /// <summary>
        /// 清空按钮点击事件，重置所有输入框和下拉框为初始状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRouteCode.Clear();
            dtpStart.Checked = false;
            dtpEnd.Checked = false;
            // 安全重置 ComboBox
            SafeResetCombo(cmbBrand);
            SafeResetCombo(cmbCategory);
            SafeResetCombo(cmbToolName);
            SafeResetCombo(cmbProductCode);
            SafeResetCombo(cmbSpec);
            SafeResetCombo(cmbTransmitStatus);

            dgvResults.DataSource = null;

            // 恢复所有控件的字体为默认状态（非加粗）
            ResetAllControlBold();
        }

        // 辅助方法：只有当 ComboBox 有项时才重置索引
        private void SafeResetCombo(ComboBox combo)
        {
            if (combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        /// <summary>
        /// 恢复所有控件的字体为默认状态（非加粗），用于清空操作后。
        /// </summary>
        private void ResetAllControlBold()
        {
            SetControlBold(txtRouteCode, false);
            SetControlBold(dtpStart, false);
            SetControlBold(dtpEnd, false);
            SetControlBold(cmbBrand, false);
            SetControlBold(cmbCategory, false);
            SetControlBold(cmbToolName, false);
            SetControlBold(cmbProductCode, false);
            SetControlBold(cmbSpec, false);
            SetControlBold(cmbTransmitStatus, false);
        }

        /// <summary>
        /// 导出EXCEL按钮点击事件，将DataGridView中的数据导出为Excel文件。可使用第三方库如EPPlus或NPOI实现。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvResults.Rows.Count == 0) return;
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel文件|*.xlsx" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("查询结果");
                    // 写标题，跳过 BarcodeId
                    int col = 1;
                    for (int i = 0; i < dgvResults.Columns.Count; i++)
                    {
                        if (dgvResults.Columns[i].Name == "BarcodeId") continue;
                        ws.Cell(1, col++).Value = dgvResults.Columns[i].HeaderText;
                    }
                    // 写数据
                    for (int i = 0; i < dgvResults.Rows.Count; i++)
                    {
                        col = 1;
                        for (int j = 0; j < dgvResults.Columns.Count; j++)
                        {
                            if (dgvResults.Columns[j].Name == "BarcodeId") continue;
                            ws.Cell(i + 2, col++).Value = dgvResults.Rows[i].Cells[j].Value?.ToString();
                        }
                    }
                    workbook.SaveAs(sfd.FileName);
                }
                MessageBox.Show("导出成功");
            }
        }

        /// <summary>
        /// 传输至打印端按钮点击事件，暂时显示提示信息，实际传输功能将在后续版本实现。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            MessageBox.Show("传输功能将在后续版本实现。");
        }


        // **********************************************
        // 以下是各个控件的事件处理器，主要用于修改控件颜色，将已选择的条件高亮显示为红色，便于用户识别当前筛选条件。
        // **********************************************



        private void txtRouteCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbToolName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
