using BarcodeMainApp.DataAccess;
using BarcodeMainApp.Models;
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

namespace BarcodeMainApp.Forms
{
    public partial class PreEntryForm : Form
    {

        private string _connStr;    // 数据库连接字符串
        private BrandRepository _brandRepo; // 用于管理品牌的仓库
        private CoatingRepository _coatingRepo; // 用于管理刀具类别、刀具名称、编号和规格的仓库
        private CategoryRepository _categoryRepo;   // 用于管理刀具类别、刀具名称、编号和规格的仓库
        private ToolNameRepository _toolNameRepo;   // 用于管理刀具类别、刀具名称、编号和规格的仓库
        private ProductCodeRepository _productCodeRepo; // 用于管理刀具类别、刀具名称、编号和规格的仓库
        private SpecRepository _specRepo;   // 用于管理刀具类别、刀具名称、编号和规格的仓库
        private CategoryToolRelationRepository _relationRepo;     // 用于管理刀具类别、刀具名称、编号和规格的关联关系
        private string _currentTableName;   // 当前选中的表名，用于新增或编辑时确定操作的表
        //private string _connectionString;  // 数据库连接字符串，用于传递给DataEditForm


        public PreEntryForm(String connStr)
        {
            InitializeComponent();
            //// 初始化数据库
            //string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BarcodeApp.db");
            //_connStr = $"Data Source={dbPath}";
            _connStr = connStr;
            _brandRepo = new BrandRepository(_connStr);
            _coatingRepo = new CoatingRepository(_connStr);
            _categoryRepo = new CategoryRepository(_connStr);
            _toolNameRepo = new ToolNameRepository(_connStr);
            _productCodeRepo = new ProductCodeRepository(_connStr);
            _specRepo = new SpecRepository(_connStr);
            _relationRepo = new CategoryToolRelationRepository(_connStr);

           
        }

        /// <summary>
        /// 当预录入界面加载时，初始化界面，包括加载刀具类别、刀具名称、编号和规格的关系树结构到TreeView控件中，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreEntryForm_Load(object sender, EventArgs e)
        {
            // 初始化界面
            LoadRelationTree();     // 加载刀具类别、刀具名称、编号和规格的关系树结构到TreeView控件中
            //LoadCategoryCombo();    // 加载刀具类别下拉框的数据源
            //LoadBrandList();     // 加载品牌列表到lstBrands控件中
            LoadAllComboBoxes();    // 加载所有下拉框的数据源，包括刀具类别、刀具名称、编号和规格
        }

        /// <summary>
        /// 加载所有下拉框的数据源，包括刀具类别、刀具名称、编号和规格，并设置显示成员和值成员，同时默认不选中任何项。
        /// </summary>
        private void LoadAllComboBoxes()
        {
            // 类别
            var categories = _categoryRepo.GetAll();
            cmbCat.DisplayMember = "Name";
            cmbCat.ValueMember = "Id";
            cmbCat.DataSource = categories;
            cmbCat.SelectedIndex = -1;

            // 刀具名称
            var toolNames = _toolNameRepo.GetAll();
            cmbTool.DisplayMember = "Name";
            cmbTool.ValueMember = "Id";
            cmbTool.DataSource = toolNames;
            cmbTool.SelectedIndex = -1;

            // 编号
            var productCodes = _productCodeRepo.GetAll();
            cmbCode.DisplayMember = "DisplayCode";
            cmbCode.ValueMember = "Id";
            cmbCode.DataSource = productCodes;
            cmbCode.SelectedIndex = -1;

            // 规格
            var specs = _specRepo.GetAll();
            cmbSpec.DisplayMember = "DisplaySpec";
            cmbSpec.ValueMember = "Id";
            cmbSpec.DataSource = specs;
            cmbSpec.SelectedIndex = -1;
        }

        ///// <summary>
        ///// 加载刀具类别下拉框的数据源，设置显示成员和值成员，并默认不选中任何项。
        ///// </summary>
        //private void LoadCategoryCombo()
        //{
        //    var categories = _categoryRepo.GetAll();
        //    cmbCat.DisplayMember = "Name";
        //    cmbCat.ValueMember = "Id";
        //    cmbCat.DataSource = categories;
        //    cmbCat.SelectedIndex = -1;
        //}


        // ***************************************************
        // 基础数据录入界面相关
        // ***************************************************


        /// <summary>
        /// 当用户在列表框中选择不同的表时，根据选择的表名从相应的仓库获取数据，并将数据绑定到DataGridView控件中显示。
        /// 基础数据录入界面，包括品牌、涂层、刀具类别、刀具名称、编号和规格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = lstTables.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected)) return;

            //switch (selected)
            //{
            //    case "品牌":
            //        dgvData.DataSource = _brandRepo.GetAll(); // 获取所有品牌数据并绑定到DataGridView
            //        break;
            //    case "涂层":
            //        dgvData.DataSource = _coatingRepo.GetAll(); // 获取所有涂层数据并绑定到DataGridView
            //        break;
            //    case "刀具类别":
            //        dgvData.DataSource = _categoryRepo.GetAll(); // 获取所有刀具类别数据并绑定到DataGridView
            //        break;
            //    case "刀具名称":
            //        dgvData.DataSource = _toolNameRepo.GetAll(); // 获取所有刀具名称数据并绑定到DataGridView
            //        break;
            //    case "编号":
            //        dgvData.DataSource = _productCodeRepo.GetAll(); // 获取所有编号数据并绑定到DataGridView
            //        break;
            //    case "规格":
            //        dgvData.DataSource = _specRepo.GetAll(); // 获取所有规格数据并绑定到DataGridView
            //        break;
            //}

            // 映射显示名到表名
            switch (selected)
            {
                case "品牌": _currentTableName = "Brand"; break;
                case "涂层": _currentTableName = "Coating"; break;
                case "刀具类别": _currentTableName = "Category"; break;
                case "刀具名称": _currentTableName = "ToolName"; break;
                case "编号": _currentTableName = "ProductCode"; break;
                case "规格": _currentTableName = "Spec"; break;
                default: return;
            }
            RefreshDataGrid();
        }

        /// <summary>
        /// 根据当前选中的表名，从相应的仓库获取数据，并将数据绑定到DataGridView控件中显示。
        /// </summary>
        private void RefreshDataGrid()
        {
            switch (_currentTableName)
            {
                case "Brand": dgvData.DataSource = _brandRepo.GetAll(); break;
                case "Coating":
                    var coatings = _coatingRepo.GetAllWithBrand();
                    dgvData.DataSource = coatings;
                    if (dgvData.Columns["BrandId"] != null)
                        dgvData.Columns["BrandId"].Visible = false;
                    break;
                case "Category": dgvData.DataSource = _categoryRepo.GetAll(); break;
                case "ToolName": dgvData.DataSource = _toolNameRepo.GetAll(); break;
                case "ProductCode": dgvData.DataSource = _productCodeRepo.GetAll(); break;
                case "Spec": dgvData.DataSource = _specRepo.GetAll(); break;
            }
            // 隐藏 Id 列，避免用户看到不连贯的数字
            if (dgvData.Columns["Id"] != null)
                dgvData.Columns["Id"].Visible = false;
            // ----- 列标题中文化 & 隐藏不需要的列 -----
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                switch (col.Name)
                {
                    case "DisplayName": col.HeaderText = "显示名称"; break;
                    case "Code": col.HeaderText = "编码"; break;
                    case "BrandName": col.HeaderText = "品牌"; break;
                    case "Name": col.HeaderText = "名称"; break;
                    case "DisplayCode": col.HeaderText = "显示编号"; break;
                    case "ConvertedCode": col.HeaderText = "转换后编号"; break;
                    case "DisplaySpec": col.HeaderText = "显示规格"; break;
                    case "Comp1": col.HeaderText = "组分1"; break;
                    case "Comp2": col.HeaderText = "组分2"; break;
                    case "Comp3": col.HeaderText = "组分3"; break;
                    case "ConvertedSpec": col.HeaderText = "转换后规格"; break;
                    case "CalcBase": col.HeaderText = "计算基数"; break;
                    case "BrandId": col.Visible = false; break;  // 隐藏外键数字
                }
            }
            // ----- 自动调整列宽 -----
            //dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// 当用户点击“新增”按钮时，弹出一个DataEditForm对话框，让用户输入新记录的详细信息，btnAdd_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentTableName)) return;
            var editForm = new DataEditForm(_currentTableName, _brandRepo);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                if (CheckDuplicate(_currentTableName, editForm.ResultEntity, null))
                {
                    SaveEntity(_currentTableName, editForm.ResultEntity);
                    RefreshDataGrid();
                }
            }
        }

        /// <summary>
        /// 当用户点击“编辑”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("请先选择一行数据"); return; }
            int id = (int)dgvData.SelectedRows[0].Cells["Id"].Value;
            object entity = GetEntityById(_currentTableName, id);
            if (entity == null) return;
            var editForm = new DataEditForm(_currentTableName, _brandRepo, entity);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                if (CheckDuplicate(_currentTableName, editForm.ResultEntity, id))
                {
                    UpdateEntity(_currentTableName, editForm.ResultEntity, id);
                    RefreshDataGrid();
                }
            }
        }

        /// <summary>
        /// 检查是否存在重复记录，如果存在则弹出提示框并返回false，否则返回true。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="excludeId"></param>
        /// <returns></returns>
        private bool CheckDuplicate(string tableName, object entity, int? excludeId)
        {
            switch (tableName)
            {
                case "Brand":
                    var brand = (Brand)entity;
                    if (_brandRepo.Exists(brand.Code, excludeId))
                    {
                        MessageBox.Show($"品牌编码 '{brand.Code}' 已存在。", "重复提示");
                        return false;
                    }
                    break;
                case "Coating":
                    var coating = (Coating)entity;
                    if (_coatingRepo.Exists(coating.BrandId, coating.Name, excludeId))
                    {
                        MessageBox.Show($"该品牌下涂层 '{coating.Name}' 已存在。", "重复提示");
                        return false;
                    }
                    break;
                case "Category":
                    var cat = (Category)entity;
                    if (_categoryRepo.Exists(cat.Name, excludeId))
                    {
                        MessageBox.Show("刀具类别名称已存在。", "重复提示");
                        return false;
                    }
                    break;
                case "ToolName":
                    var tool = (ToolName)entity;
                    if (_toolNameRepo.Exists(tool.Name, excludeId))
                    {
                        MessageBox.Show("刀具名称已存在。", "重复提示");
                        return false;
                    }
                    break;
                case "ProductCode":
                    var pc = (ProductCode)entity;
                    if (_productCodeRepo.Exists(pc.DisplayCode, excludeId))
                    {
                        MessageBox.Show($"编号 '{pc.DisplayCode}' 已存在。", "重复提示");
                        return false;
                    }
                    break;
                case "Spec":
                    var spec = (Spec)entity;
                    if (_specRepo.ExistsByComponents(spec.Comp1, spec.Comp2, spec.Comp3, excludeId))
                    {
                        MessageBox.Show("相同计算组分的规格已存在，请检查。", "重复提示");
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 根据表名和ID获取对应的实体对象，用于编辑操作时从数据库中获取原始数据。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private object GetEntityById(string tableName, int id)
        {
            switch (tableName)
            {
                case "Brand": return _brandRepo.GetById(id);
                case "Coating": return _coatingRepo.GetById(id);
                case "Category": return _categoryRepo.GetById(id);
                case "ToolName": return _toolNameRepo.GetById(id);
                case "ProductCode": return _productCodeRepo.GetById(id);
                case "Spec": return _specRepo.GetById(id);
                default: return null;
            }
        }

        /// <summary>
        /// 根据表名和实体对象，将新记录保存到数据库中，调用相应的仓库的Add方法。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        private void SaveEntity(string tableName, object entity)
        {
            switch (tableName)
            {
                case "Brand": _brandRepo.Add((Brand)entity); break;
                case "Coating": _coatingRepo.Add((Coating)entity); break;
                case "Category": _categoryRepo.Add((Category)entity); break;
                case "ToolName": _toolNameRepo.Add((ToolName)entity); break;
                case "ProductCode": _productCodeRepo.Add((ProductCode)entity); break;
                case "Spec": _specRepo.Add((Spec)entity); break;
            }
        }

        /// <summary>
        /// 根据表名、实体对象和ID，将编辑后的记录更新到数据库中，调用相应的仓库的Update方法。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        private void UpdateEntity(string tableName, object entity, int id)
        {
            // 各 Repository 需要有 Update 方法，需要事先添加
            switch (tableName)
            {
                case "Brand": _brandRepo.Update((Brand)entity, id); break;
                case "Coating": _coatingRepo.Update((Coating)entity, id); break;
                case "Category": _categoryRepo.Update((Category)entity, id); break;
                case "ToolName": _toolNameRepo.Update((ToolName)entity, id); break;
                case "ProductCode": _productCodeRepo.Update((ProductCode)entity, id); break;
                case "Spec": _specRepo.Update((Spec)entity, id); break;
            }
        }

        /// <summary>
        /// 当用户点击“删除”按钮时，获取当前选中的行的ID，并调用相应的仓库的Delete方法将该记录从数据库中删除，然后刷新DataGridView控件显示。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("请先选择一行数据"); return; }
            if (MessageBox.Show("确定删除吗？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            int id = (int)dgvData.SelectedRows[0].Cells["Id"].Value;
            DeleteEntity(_currentTableName, id);
            RefreshDataGrid();
        }

        private void DeleteEntity(string tableName, int id)
        {
            switch (tableName)
            {
                case "Brand": _brandRepo.Delete(id); break;
                case "Coating": _coatingRepo.Delete(id); break;
                case "Category": _categoryRepo.Delete(id); break;
                case "ToolName": _toolNameRepo.Delete(id); break;
                case "ProductCode": _productCodeRepo.Delete(id); break;
                case "Spec": _specRepo.Delete(id); break;
            }
        }


        // *****************************************************
        // 关联关系录入界面相关
        // *****************************************************


        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 当用户在TreeView控件中选择不同的节点时，加载刀具类别、刀具名称、编号和规格的关系树结构到TreeView控件中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvRelations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        /// <summary>
        /// 加载刀具类别、刀具名称、编号和规格的关系树结构到TreeView控件中。
        /// </summary>
        private void LoadRelationTree()
        {
            tvRelations.Nodes.Clear(); // 清空现有节点
            var categories = _categoryRepo.GetAll(); // 获取所有刀具类别
            foreach (var cat in categories)
            {
                var catNode = tvRelations.Nodes.Add(cat.Name);
                catNode.Tag = cat.Id;

                var tools = _relationRepo.GetToolNamesByCategory(cat.Id);  // 获取该类别下的所有刀具名称
                foreach (var tool in tools)
                {
                    var toolNode = catNode.Nodes.Add(tool.Name); // 添加刀具名称节点
                    toolNode.Tag = new { CategoryId = cat.Id, ToolNameId = tool.Id }; // 添加刀具名称节点的Tag，包含类别ID和刀具名称ID

                    var codes = _relationRepo.GetCodesByCategoryAndTool(cat.Id, tool.Id); // 获取该类别和刀具名称下的所有编号
                    foreach (var code in codes)
                    {
                        var codeNode = toolNode.Nodes.Add(code.DisplayCode); // 添加编号节点
                        codeNode.Tag = new { CategoryId = cat.Id, ToolNameId = tool.Id, ProductCodeId = code.Id }; // 添加编号节点的Tag，包含类别ID、刀具名称ID和编号ID

                        var specs = _relationRepo.GetSpecsByCategoryToolAndCode(cat.Id, tool.Id, code.Id); // 获取该类别、刀具名称和编号下的所有规格
                        foreach (var spec in specs)
                        {
                            var specNode = codeNode.Nodes.Add(spec.DisplaySpec); // 添加规格节点
                            specNode.Tag = new { CategoryId = cat.Id, ToolNameId = tool.Id, ProductCodeId = code.Id, SpecId = spec.Id }; // 添加规格节点的Tag，包含类别ID、刀具名称ID、编号ID和规格ID
                        }
                    }
                }
            }
            tvRelations.ExpandAll(); // 展开所有节点，方便查看整个关系树
        }

        /// <summary>
        /// 当用户在刀具类别下拉框中选择不同的类别时，根据选择的类别ID获取该类别下的刀具名称列表，并绑定到刀具名称下拉框中，同时清空编号和规格下拉框的数据源。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddRelation.Enabled = cmbCat.SelectedIndex >= 0
                           && cmbTool.SelectedIndex >= 0
                           && cmbCode.SelectedIndex >= 0
                           && cmbSpec.SelectedIndex >= 0;
        }

        /// <summary>
        /// 当用户在刀具名称下拉框中选择不同的刀具名称时，根据选择的类别ID和刀具名称ID获取该类别和刀具名称下的编号列表，并绑定到编号下拉框中，同时清空规格下拉框的数据源。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddRelation.Enabled = cmbCat.SelectedIndex >= 0
                           && cmbTool.SelectedIndex >= 0
                           && cmbCode.SelectedIndex >= 0
                           && cmbSpec.SelectedIndex >= 0;
        }

        /// <summary>
        /// 当用户在编号下拉框中选择不同的编号时，根据选择的类别ID、刀具名称ID和编号ID获取该类别、刀具名称和编号下的规格列表，并绑定到规格下拉框中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddRelation.Enabled = cmbCat.SelectedIndex >= 0
                           && cmbTool.SelectedIndex >= 0
                           && cmbCode.SelectedIndex >= 0
                           && cmbSpec.SelectedIndex >= 0;
        }

        /// <summary>
        /// 当用户在规格下拉框中选择不同的规格时，可以在此方法中处理相关逻辑，例如显示选中的规格信息或进行其他操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当规格也选择后，启用添加按钮
            btnAddRelation.Enabled = cmbCat.SelectedIndex >= 0
                                   && cmbTool.SelectedIndex >= 0
                                   && cmbCode.SelectedIndex >= 0
                                   && cmbSpec.SelectedIndex >= 0;
        }

        /// <summary>
        /// 当用户点击“添加关联”按钮时，获取用户在下拉框中选择的类别、刀具名称、编号和规格的ID，
        /// 并调用CategoryToolRelationRepository的Add方法将关联关系添加到数据库中，然后刷新关系树并提示用户关联添加成功。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRelation_Click(object sender, EventArgs e)
        {
            if (cmbCat.SelectedValue == null || cmbTool.SelectedValue == null
        || cmbCode.SelectedValue == null || cmbSpec.SelectedValue == null)
            {
                MessageBox.Show("请完整选择类别、刀具、编号和规格。");
                return;
            }
            int catId = (int)cmbCat.SelectedValue;
            int toolId = (int)cmbTool.SelectedValue;
            int codeId = (int)cmbCode.SelectedValue;
            int specId = (int)cmbSpec.SelectedValue;

            _relationRepo.Add(catId, toolId, codeId, specId);
            LoadRelationTree();
            MessageBox.Show("关联添加成功。");
        }

        /// <summary>
        /// 当用户点击“删除关联”按钮时，获取用户在TreeView中选中的节点，如果选中的节点是最底层的规格节点，
        /// 则获取该节点的类别ID、刀具名称ID、编号ID和规格ID，
        /// 并调用CategoryToolRelationRepository的Delete方法将关联关系从数据库中删除，然后刷新关系树并提示用户关联删除成功。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteRelation_Click(object sender, EventArgs e)
        {
            var selectedNode = tvRelations.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("请先在树中选中一个关联节点。");
                return;
            }
            // 只有最底层的规格节点才有关联记录
            if (selectedNode.Tag == null || selectedNode.Parent == null || selectedNode.Parent.Parent == null)
            {
                MessageBox.Show("请选中一个具体的规格节点（最底层）。");
                return;
            }
            dynamic tag = selectedNode.Tag;
            int catId = tag.CategoryId;
            int toolId = tag.ToolNameId;
            int codeId = tag.ProductCodeId;
            int specId = tag.SpecId;

            if (MessageBox.Show("确定要删除此关联吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _relationRepo.Delete(catId, toolId, codeId, specId);
                LoadRelationTree();
            }
        }

       
    }
}   
