using BarcodeMainApp.DataAccess;
using BarcodeMainApp.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeMainApp.Forms
{
    public partial class BarcodeEntryForm : Form
    {
        // 数据库连接字符串
        private string _connectionString;
        // 各类仓库
        private BrandRepository _brandRepo;
        private CoatingRepository _coatingRepo;
        private CategoryRepository _categoryRepo;
        private ToolNameRepository _toolNameRepo;
        private ProductCodeRepository _productCodeRepo;
        private SpecRepository _specRepo;
        private CategoryToolRelationRepository _relationRepo;
        private BarcodeRepository _barcodeRepo;

        // 用于记录当前生成的完整条码，以便保存时使用
        private string _currentBarcode;
        // 为避免递归调用，可以在 TextChanged 中临时解绑事件再绑定，但直接修改 Text 不会再次触发 TextChanged
        private bool _isUpdatingRoute;
        // 在 txtQuantity 的 KeyPress 和 TextChanged 中加入范围检查。
        private bool _isUpdatingQty;
        // 记录当前是否正在保存数据，防止重复点击保存按钮导致多次保存
        private bool _isSaving = false;
        private bool _isLoading = false;
        private bool _isClearing = false;



        public BarcodeEntryForm(string connectionString)
        {
            InitializeComponent();          // 必须先创建控件
            _connectionString = connectionString;
            InitializeRepos();              // 初始化仓库
            BindEvents();                   // 绑定 Load 等事件
        }

        /// <summary>
        /// 初始化各类仓库实例
        /// </summary>
        private void InitializeRepos()
        {
            _brandRepo = new BrandRepository(_connectionString);
            _coatingRepo = new CoatingRepository(_connectionString);
            _categoryRepo = new CategoryRepository(_connectionString);
            _toolNameRepo = new ToolNameRepository(_connectionString);
            _productCodeRepo = new ProductCodeRepository(_connectionString);
            _specRepo = new SpecRepository(_connectionString);
            _relationRepo = new CategoryToolRelationRepository(_connectionString);
            _barcodeRepo = new BarcodeRepository(_connectionString);
        }

        /// <summary>
        /// 绑定各控件的事件处理程序
        /// </summary>
        private void BindEvents()
        {
            // 先解绑，防止重复绑定
            this.Load += BarcodeEntryForm_Load;
            cmbBrand.SelectedIndexChanged -= cmbBrand_SelectedIndexChanged;
            cmbBrand.SelectedIndexChanged += cmbBrand_SelectedIndexChanged;
            //cmbBrand.SelectedIndexChanged += UpdateBarcodePreview;
            cmbCoating.SelectedIndexChanged -= cmbCoating_SelectedIndexChanged;
            cmbCoating.SelectedIndexChanged += UpdateBarcodePreview;
            cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            cmbToolName.SelectedIndexChanged -= cmbToolName_SelectedIndexChanged;
            cmbToolName.SelectedIndexChanged += cmbToolName_SelectedIndexChanged;
            cmbProductCode.SelectedIndexChanged -= cmbProductCode_SelectedIndexChanged;
            cmbProductCode.SelectedIndexChanged += cmbProductCode_SelectedIndexChanged;
            cmbSpec.SelectedIndexChanged -= UpdateBarcodePreview;
            cmbSpec.SelectedIndexChanged += UpdateBarcodePreview;
            txtRouteCode.TextChanged -= UpdateBarcodePreview;
            txtRouteCode.TextChanged += UpdateBarcodePreview;
            txtQuantity.TextChanged -= UpdateBarcodePreview;
            txtQuantity.TextChanged += UpdateBarcodePreview;
            btnSave.Click -= btnSave_Click;
            btnSave.Click += btnSave_Click;
            btnSend.Click -= btnSend_Click;
            btnSend.Click += btnSend_Click;
            btnExport.Click -= btnExport_Click;
            btnExport.Click += btnExport_Click;
            btnOpenPreset.Click -= btnOpenPreset_Click;
            btnOpenPreset.Click += btnOpenPreset_Click;
            dgvTodayRecords.CellDoubleClick -= dgvTodayRecords_CellDoubleClick;
            dgvTodayRecords.CellDoubleClick += dgvTodayRecords_CellDoubleClick;
            tsmiEditQuantity.Click -= tsmiEditQuantity_Click;
            tsmiEditQuantity.Click += tsmiEditQuantity_Click;
            tsmiDelete.Click -= tsmiDelete_Click;
            tsmiDelete.Click += tsmiDelete_Click;
        }

        private Label GetLblDate1()
        {
            return lblDate;
        }

        /// <summary>
        /// 窗口加载事件处理程序，用于初始化界面和加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarcodeEntryForm_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            var lblDate1 = GetLblDate1();
            lblDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // 2. 加载品牌下拉框
            var brands = _brandRepo.GetAll();
            if (brands.Count > 0)
            {
                cmbBrand.DataSource = brands;
                cmbBrand.DisplayMember = "DisplayName";
                cmbBrand.ValueMember = "Id";
            }
            cmbBrand.SelectedIndex = -1;

            // 3. 加载类别下拉框
            var categories = _categoryRepo.GetAll();
            if (categories.Count > 0)
            {
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id";
            }
            cmbCategory.SelectedIndex = -1;

            _isLoading = false;

            // 4. 刷新今日记录
            RefreshTodayRecords();
        }

        /// <summary>
        /// 主面板绘制事件处理程序，用于自定义绘制或其他图形操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblMain_Paint(object sender, PaintEventArgs e)
        {

        }

        // ****************************************************
        // 输入控件相关事件处理程序
        // ****************************************************

        /// <summary>
        /// 路线单号输入框按键按下事件处理程序，用于限制输入为数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //// 只允许数字、退格键
            //if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            //    e.Handled = true;
        }

        /// <summary>
        /// 路线单号输入框文本改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteCode_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingRoute) return;
            _isUpdatingRoute = true;
            string digits = new string(txtRouteCode.Text.Where(char.IsDigit).ToArray());
            if (digits.Length > 7) digits = digits.Substring(0, 7);
            txtRouteCode.Text = digits;
            txtRouteCode.SelectionStart = digits.Length;
            _isUpdatingRoute = false;
            UpdateBarcodePreview(sender, e);
        }

        /// <summary>
        /// 数量输入框文本改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingQty) return;
            _isUpdatingQty = true;
            // 移除非数字
            string digits = new string(txtQuantity.Text.Where(char.IsDigit).ToArray());
            if (digits.Length > 3) digits = digits.Substring(0, 3);
            txtQuantity.Text = digits;
            txtQuantity.SelectionStart = digits.Length;
            _isUpdatingQty = false;
            UpdateBarcodePreview(sender, e);
        }

        /// <summary>
        /// 数量输入框按键按下事件处理程序，用于限制输入为数字和控制键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //// 只允许数字和控制键（退格、删除等）
            //if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            //    e.Handled = true;
        }

        /// <summary>
        /// 品牌下拉框选中项改变事件处理程序，品牌变更 → 过滤涂层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (cmbBrand.SelectedValue == null)
            {
                cmbCoating.DataSource = null;
                return;
            }
            if (!(cmbBrand.SelectedItem is Brand brand)) return;
            int brandId = brand.Id;
            var coatings = _coatingRepo.GetByBrandId(brandId);
            cmbCoating.DataSource = coatings;
            cmbCoating.DisplayMember = "Name";
            cmbCoating.ValueMember = "Id";
            cmbCoating.SelectedIndex = -1;
        }

        /// <summary>
        /// 涂层下拉框选中项改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCoating_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 类别下拉框选中项改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (cmbCategory.SelectedValue == null) return;
            if (!(cmbCategory.SelectedItem is Category cat)) return;
            int catId = cat.Id;

            var tools = _relationRepo.GetToolNamesByCategory(catId);
            cmbToolName.DisplayMember = "Name";
            cmbToolName.ValueMember = "Id";
            cmbToolName.DataSource = tools;
            cmbToolName.SelectedIndex = -1;
            cmbProductCode.DataSource = null;
            cmbSpec.DataSource = null;
            UpdateBarcodePreview(sender, e);
        }

        /// <summary>
        /// 刀具名称下拉框选中项改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbToolName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (!(cmbCategory.SelectedItem is Category cat) || !(cmbToolName.SelectedItem is ToolName tool)) return;
            int catId = cat.Id;
            int toolId = tool.Id;

            var codes = _relationRepo.GetCodesByCategoryAndTool(catId, toolId);
            cmbProductCode.DisplayMember = "DisplayCode";
            cmbProductCode.ValueMember = "Id";
            cmbProductCode.DataSource = codes;
            cmbProductCode.SelectedIndex = -1;
            cmbSpec.DataSource = null;
            UpdateBarcodePreview(sender, e);
        }

        /// <summary>
        /// 编号下拉框选中项改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (cmbProductCode.SelectedValue == null || cmbCategory.SelectedValue == null || cmbToolName.SelectedValue == null) return;
            if (!(cmbCategory.SelectedItem is Category cat) || !(cmbToolName.SelectedItem is ToolName tool) || !(cmbProductCode.SelectedItem is ProductCode code)) return;
            int catId = cat.Id;
            int toolId = tool.Id;
            int codeId = code.Id;

            var specs = _relationRepo.GetSpecsByCategoryToolAndCode(catId, toolId, codeId);
            cmbSpec.DisplayMember = "DisplaySpec";
            cmbSpec.ValueMember = "Id";
            cmbSpec.DataSource = specs;
            cmbSpec.SelectedIndex = -1;
            UpdateBarcodePreview(sender, e);
        }

        /// <summary>
        /// 规格下拉框选中项改变事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 【保存记录】按钮点击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving) return;
            _isSaving = true;

            if (string.IsNullOrEmpty(_currentBarcode))
            {
                MessageBox.Show("条码尚未生成或生成失败，请检查输入。");
                return;
            }

            try
            {
                var barcode = new Barcode
                {
                    RouteCode = txtRouteCode.Text.Trim(),
                    CategoryId = (int)cmbCategory.SelectedValue,
                    ToolNameId = (int)cmbToolName.SelectedValue,
                    BrandId = (int)cmbBrand.SelectedValue,
                    CoatingId = (int)cmbCoating.SelectedValue,
                    ProductCodeId = (int)cmbProductCode.SelectedValue,
                    SpecId = (int)cmbSpec.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    FullBarcode = _currentBarcode,
                    OrderDate = lblDate.Text,
                    CreatedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                var code = (ProductCode)cmbProductCode.SelectedItem;
                var toolName = (ToolName)cmbToolName.SelectedItem;
                var spec = (Spec)cmbSpec.SelectedItem;
                var coating = (Coating)cmbCoating.SelectedItem;


                int barcodeId = _barcodeRepo.Add(barcode);

                //MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                RefreshTodayRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isSaving = false;
            }
        }

        /// <summary>
        /// 清除表单内容，重置输入控件
        /// </summary>
        private void ClearForm()
        {
            // 暂停条码更新
            _isClearing = true;

            txtRouteCode.Clear();
            txtQuantity.Text = "1";
            cmbCategory.SelectedIndex = -1;
            cmbToolName.DataSource = null;
            cmbProductCode.DataSource = null;
            cmbSpec.DataSource = null;
            cmbBrand.SelectedIndex = -1;
            cmbCoating.DataSource = null;
            lblBarcodePreview.Text = "";
            _currentBarcode = null;

            _isClearing = false;
            // 最终刷新一次
            UpdateBarcodePreview(this, EventArgs.Empty);
        }


        // ****************************************************
        // 已录入数据表格相关事件处理程序
        // ****************************************************

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTodayRecords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        /// <summary>
        /// 双击数据表格行事件处理程序，用于快速编辑数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTodayRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvTodayRecords.Rows[e.RowIndex];
            int barcodeId = (int)row.Cells["BarcodeId"].Value;

            // 弹出数量修改窗口
            string input = Microsoft.VisualBasic.Interaction.InputBox("请输入新的数量：", "修改数量", row.Cells["数量"].Value.ToString());
            if (string.IsNullOrEmpty(input)) return;
            if (int.TryParse(input, out int newQty) && newQty > 0)
            {
                try
                {
                    // 获取原条码记录
                    var barcode = _barcodeRepo.GetById(barcodeId);
                    if (barcode != null)
                    {
                        barcode.Quantity = newQty;
                        // 重新生成条码：数量改变，条码需要更新，但路线单号、编号等不变。
                        // 简便起见，我们重新拼接条码并更新
                        var productCode = _productCodeRepo.GetById(barcode.ProductCodeId);
                        var spec = _specRepo.GetById(barcode.SpecId);
                        var brand = _brandRepo.GetById(barcode.BrandId);
                        string datePart = DateTime.Parse(barcode.OrderDate).ToString("yyMMdd");
                        barcode.FullBarcode = $"{barcode.RouteCode}{productCode.ConvertedCode}{spec.ConvertedSpec}{newQty:D3}{datePart}{brand.Code}";
                        _barcodeRepo.Update(barcode);

                        RefreshTodayRecords();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("更新失败：" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("数量输入无效。");
            }
        }

        /// <summary>
        /// 双击数据表格行事件处理程序，用于快速编辑数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiEditQuantity_Click(object sender, EventArgs e)
        {
            if (dgvTodayRecords.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvTodayRecords.SelectedRows[0];
                dgvTodayRecords_CellDoubleClick(this, new DataGridViewCellEventArgs(row.Cells[0].ColumnIndex, row.Index));
            }
        }

        /// <summary>
        /// 双击数据表格行事件处理程序，用于删除记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (dgvTodayRecords.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择一条记录。");
                return;
            }

            int barcodeId = (int)dgvTodayRecords.SelectedRows[0].Cells["BarcodeId"].Value;

            if (MessageBox.Show("确定要删除这条记录吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    _barcodeRepo.Delete(barcodeId);               // 再删条码
                    RefreshTodayRecords();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 【传输至打印端】按钮点击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            MessageBox.Show("传输至打印端功能将在后续版本实现。", "提示");
        }

        /// <summary>
        /// 【导出EXCEL】按钮点击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvTodayRecords.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可导出。");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel文件|*.xlsx",
                FileName = $"条码记录_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("当天记录");
                        // 标题行
                        for (int i = 0; i < dgvTodayRecords.Columns.Count; i++)
                        {
                            if (dgvTodayRecords.Columns[i].Name == "BarcodeId") continue;  // 跳过隐藏列
                            ws.Cell(1, i + 1).Value = dgvTodayRecords.Columns[i].HeaderText;
                        }
                        // 数据行
                        for (int i = 0; i < dgvTodayRecords.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvTodayRecords.Columns.Count; j++)
                            {
                                if (dgvTodayRecords.Columns[j].Name == "BarcodeId") continue;  // 跳过隐藏列
                                var cellValue = dgvTodayRecords.Rows[i].Cells[j].Value;
                                ws.Cell(i + 2, j + 1).Value = cellValue?.ToString() ?? "";
                            }
                        }
                        workbook.SaveAs(sfd.FileName);
                    }
                    MessageBox.Show("导出成功！", "提示");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }

            }
        }

        /// <summary>
        /// 【打开预设数据界面】按钮点击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenPreset_Click(object sender, EventArgs e)
        {
            //PreEntryForm preForm = new PreEntryForm(_connectionString);
            //preForm.ShowDialog();
            //// 刷新当前下拉框数据
            //cmbCategory.DataSource = _categoryRepo.GetAll();
            //cmbBrand.DataSource = _brandRepo.GetAll();
            //cmbBrand.SelectedIndex = -1;
            //cmbCategory.SelectedIndex = -1;

            this.Hide(); // 隐藏自己

            using (PreEntryForm preForm = new PreEntryForm(_connectionString))
            {
                preForm.ShowDialog(); // 模态打开预录窗口
            }

            // 预录窗口关闭后，重新显示并刷新数据
            this.Show();
            RefreshAfterChildClosed();
        }

        /// <summary>
        /// 【查询历史数据】按钮点击事件处理程序，打开查询窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSearch_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (SearchForm searchForm = new SearchForm(_connectionString))
            {
                searchForm.ShowDialog();
            }

            this.Show();
            RefreshAfterChildClosed();
        }

        // 刷新界面数据的方法
        private void RefreshAfterChildClosed()
        {
            // 重新加载品牌和类别下拉框，以同步预录窗口的修改
            cmbBrand.DataSource = _brandRepo.GetAll();
            cmbCategory.DataSource = _categoryRepo.GetAll();

            // 清空当前已选中的产品信息
            ClearForm();
            RefreshTodayRecords();
        }


        // ****************************************************
        // 调用方法
        // ****************************************************

        // ===================== 条码预览生成 =====================
        private void UpdateBarcodePreview(object sender, EventArgs e)
        {
            if (_isLoading || _isClearing) return;

            // 检查所有必填项是否完整
            if (string.IsNullOrWhiteSpace(txtRouteCode.Text) || txtRouteCode.Text.Length != 7)
            {
                lblBarcodePreview.Text = "路线单号必须7位";
                _currentBarcode = null;
                return;
            }
            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                lblBarcodePreview.Text = "数量无效";
                _currentBarcode = null;
                return;
            }
            if (cmbCategory.SelectedIndex < 0 || cmbToolName.SelectedIndex < 0 ||
                cmbProductCode.SelectedIndex < 0 || cmbSpec.SelectedIndex < 0 ||
                cmbBrand.SelectedIndex < 0 || cmbCoating.SelectedIndex < 0)
            {
                lblBarcodePreview.Text = "请完整选择产品信息";
                _currentBarcode = null;
                return;
            }

            try
            {
                var code = (ProductCode)cmbProductCode.SelectedItem;
                var spec = (Spec)cmbSpec.SelectedItem;
                var brand = (Brand)cmbBrand.SelectedItem;
                string route = txtRouteCode.Text.Trim();
                string datePart = DateTime.Now.ToString("yyMMdd");  // 或者用lblDate的文本转换，但实际我们保存用DateTimePicker？这里没有DateTimePicker，我们采用当天日期。
                // 之前我们用了lblDate显示当天日期，这里直接使用lblDate.Text格式化
                DateTime orderDate;
                if (!DateTime.TryParse(lblDate.Text, out orderDate))
                    orderDate = DateTime.Now;
                datePart = orderDate.ToString("yyMMdd");

                _currentBarcode = $"{route}{code.ConvertedCode}{spec.ConvertedSpec}{qty:D3}{datePart}{brand.Code}";
                lblBarcodePreview.Text = _currentBarcode;
            }
            catch (Exception ex)
            {
                lblBarcodePreview.Text = "生成错误：" + ex.Message;
                _currentBarcode = null;
            }
        }

        // ===================== 当天记录列表 =====================
        private void RefreshTodayRecords()
        {
            // 查询当天日期的所有条码记录（忽略时间部分）
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var list = _barcodeRepo.GetByDate(today);  // 需要在BarcodeRepository添加GetByDate方法

            if (list != null)
            {
                // 转换为视图模型方便显示
                var viewModels = list.Select(b => new
                {
                    路线单号 = b.RouteCode,
                    品牌 = _brandRepo.GetById(b.BrandId)?.DisplayName ?? "",
                    编号 = _productCodeRepo.GetById(b.ProductCodeId)?.DisplayCode ?? "",
                    规格 = _specRepo.GetById(b.SpecId)?.DisplaySpec ?? "",
                    数量 = b.Quantity,
                    条码 = b.FullBarcode,
                    传输状态 = b.IsTransmitted ? "已传输" : "未传输",
                    BarcodeId = b.Id
                }).ToList();
                dgvTodayRecords.DataSource = viewModels;
                // 隐藏BarcodeId列
                if (dgvTodayRecords.Columns["BarcodeId"] != null)
                    dgvTodayRecords.Columns["BarcodeId"].Visible = false;
            }
            else
            {
                dgvTodayRecords.DataSource = null;
            }

            dgvTodayRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // 然后再将其他列设置为 Fill 模式（除条码外）
            foreach (DataGridViewColumn col in dgvTodayRecords.Columns)
            {
                if (col.Name == "条码")
                    col.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.AllCells;
                else
                    col.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        
    }
}
