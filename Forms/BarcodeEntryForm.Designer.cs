namespace BarcodeMainApp.Forms
{
    partial class BarcodeEntryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.tblInput = new System.Windows.Forms.TableLayoutPanel();
            this.tblTop = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.baseInputTb = new System.Windows.Forms.TableLayoutPanel();
            this.txtRouteCode = new AntdUI.Input();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQuantity = new AntdUI.Input();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pToolTb = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBrand = new System.Windows.Forms.ComboBox();
            this.cmbCoating = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.specTb = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbToolName = new System.Windows.Forms.ComboBox();
            this.cmbProductCode = new System.Windows.Forms.ComboBox();
            this.cmbSpec = new System.Windows.Forms.ComboBox();
            this.tblBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new AntdUI.Button();
            this.lblBarcodePreview = new System.Windows.Forms.Label();
            this.listTb = new System.Windows.Forms.TableLayoutPanel();
            this.dgvTodayRecords = new System.Windows.Forms.DataGridView();
            this.cmsRecords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiEditQuantity = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlListActions = new System.Windows.Forms.Panel();
            this.btnOpenSearch = new AntdUI.Button();
            this.btnOpenPreset = new AntdUI.Button();
            this.btnExport = new AntdUI.Button();
            this.btnSend = new AntdUI.Button();
            this.tblMain.SuspendLayout();
            this.tblInput.SuspendLayout();
            this.tblTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.baseInputTb.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pToolTb.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.specTb.SuspendLayout();
            this.tblBottom.SuspendLayout();
            this.listTb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayRecords)).BeginInit();
            this.cmsRecords.SuspendLayout();
            this.pnlListActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.tblInput, 0, 0);
            this.tblMain.Controls.Add(this.listTb, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(984, 761);
            this.tblMain.TabIndex = 0;
            this.tblMain.Paint += new System.Windows.Forms.PaintEventHandler(this.tblMain_Paint);
            // 
            // tblInput
            // 
            this.tblInput.ColumnCount = 1;
            this.tblInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblInput.Controls.Add(this.tblTop, 0, 0);
            this.tblInput.Controls.Add(this.tblBottom, 0, 1);
            this.tblInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblInput.Location = new System.Drawing.Point(3, 3);
            this.tblInput.Name = "tblInput";
            this.tblInput.RowCount = 2;
            this.tblInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tblInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblInput.Size = new System.Drawing.Size(978, 294);
            this.tblInput.TabIndex = 0;
            // 
            // tblTop
            // 
            this.tblTop.ColumnCount = 3;
            this.tblTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tblTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tblTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTop.Controls.Add(this.groupBox1, 0, 0);
            this.tblTop.Controls.Add(this.groupBox2, 1, 0);
            this.tblTop.Controls.Add(this.groupBox3, 2, 0);
            this.tblTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTop.Location = new System.Drawing.Point(3, 3);
            this.tblTop.Name = "tblTop";
            this.tblTop.RowCount = 1;
            this.tblTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTop.Size = new System.Drawing.Size(972, 224);
            this.tblTop.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.baseInputTb);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 218);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本信息";
            // 
            // baseInputTb
            // 
            this.baseInputTb.ColumnCount = 2;
            this.baseInputTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.baseInputTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.baseInputTb.Controls.Add(this.txtRouteCode, 1, 0);
            this.baseInputTb.Controls.Add(this.label1, 0, 0);
            this.baseInputTb.Controls.Add(this.label2, 0, 1);
            this.baseInputTb.Controls.Add(this.txtQuantity, 1, 1);
            this.baseInputTb.Controls.Add(this.label3, 0, 2);
            this.baseInputTb.Controls.Add(this.lblDate, 1, 2);
            this.baseInputTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseInputTb.Location = new System.Drawing.Point(3, 27);
            this.baseInputTb.Name = "baseInputTb";
            this.baseInputTb.RowCount = 4;
            this.baseInputTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.baseInputTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.baseInputTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.baseInputTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.baseInputTb.Size = new System.Drawing.Size(288, 188);
            this.baseInputTb.TabIndex = 0;
            // 
            // txtRouteCode
            // 
            this.txtRouteCode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtRouteCode.Location = new System.Drawing.Point(98, 19);
            this.txtRouteCode.MaxLength = 7;
            this.txtRouteCode.Name = "txtRouteCode";
            this.txtRouteCode.Size = new System.Drawing.Size(187, 28);
            this.txtRouteCode.TabIndex = 0;
            this.txtRouteCode.TextChanged += new System.EventHandler(this.txtRouteCode_TextChanged);
            this.txtRouteCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRouteCode_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "路线单号：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 50);
            this.label2.TabIndex = 2;
            this.label2.Text = "数量：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtQuantity.Location = new System.Drawing.Point(98, 69);
            this.txtQuantity.MaxLength = 3;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(187, 28);
            this.txtQuantity.TabIndex = 3;
            this.txtQuantity.Text = "1";
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
            this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 50);
            this.label3.TabIndex = 4;
            this.label3.Text = "日期：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Location = new System.Drawing.Point(98, 100);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(187, 50);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "当天日期";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pToolTb);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(303, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 218);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "品牌-涂层";
            // 
            // pToolTb
            // 
            this.pToolTb.ColumnCount = 2;
            this.pToolTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.pToolTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pToolTb.Controls.Add(this.label4, 0, 0);
            this.pToolTb.Controls.Add(this.label5, 0, 1);
            this.pToolTb.Controls.Add(this.cmbBrand, 1, 0);
            this.pToolTb.Controls.Add(this.cmbCoating, 1, 1);
            this.pToolTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pToolTb.Location = new System.Drawing.Point(3, 27);
            this.pToolTb.Name = "pToolTb";
            this.pToolTb.RowCount = 3;
            this.pToolTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pToolTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pToolTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.pToolTb.Size = new System.Drawing.Size(288, 188);
            this.pToolTb.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 69);
            this.label4.TabIndex = 0;
            this.label4.Text = "品牌：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 69);
            this.label5.TabIndex = 1;
            this.label5.Text = "涂层：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // cmbBrand
            // 
            this.cmbBrand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrand.FormattingEnabled = true;
            this.cmbBrand.Location = new System.Drawing.Point(83, 40);
            this.cmbBrand.Name = "cmbBrand";
            this.cmbBrand.Size = new System.Drawing.Size(202, 32);
            this.cmbBrand.TabIndex = 2;
            this.cmbBrand.SelectedIndexChanged += new System.EventHandler(this.cmbBrand_SelectedIndexChanged);
            // 
            // cmbCoating
            // 
            this.cmbCoating.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbCoating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoating.FormattingEnabled = true;
            this.cmbCoating.Location = new System.Drawing.Point(83, 109);
            this.cmbCoating.Name = "cmbCoating";
            this.cmbCoating.Size = new System.Drawing.Size(202, 32);
            this.cmbCoating.TabIndex = 3;
            this.cmbCoating.SelectedIndexChanged += new System.EventHandler(this.cmbCoating_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.specTb);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(603, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(366, 218);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "产品选择";
            // 
            // specTb
            // 
            this.specTb.ColumnCount = 2;
            this.specTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.specTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.specTb.Controls.Add(this.label6, 0, 0);
            this.specTb.Controls.Add(this.label7, 0, 1);
            this.specTb.Controls.Add(this.label8, 0, 2);
            this.specTb.Controls.Add(this.label9, 0, 3);
            this.specTb.Controls.Add(this.cmbCategory, 1, 0);
            this.specTb.Controls.Add(this.cmbToolName, 1, 1);
            this.specTb.Controls.Add(this.cmbProductCode, 1, 2);
            this.specTb.Controls.Add(this.cmbSpec, 1, 3);
            this.specTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.specTb.Location = new System.Drawing.Point(3, 27);
            this.specTb.Name = "specTb";
            this.specTb.RowCount = 5;
            this.specTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.specTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.specTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.specTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.specTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.specTb.Size = new System.Drawing.Size(360, 188);
            this.specTb.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 40);
            this.label6.TabIndex = 0;
            this.label6.Text = "类别：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 40);
            this.label7.TabIndex = 1;
            this.label7.Text = "刀具名称：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 40);
            this.label8.TabIndex = 2;
            this.label8.Text = "编号：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 38);
            this.label9.TabIndex = 3;
            this.label9.Text = "规格：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(93, 11);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(264, 32);
            this.cmbCategory.TabIndex = 4;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // cmbToolName
            // 
            this.cmbToolName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbToolName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToolName.FormattingEnabled = true;
            this.cmbToolName.Location = new System.Drawing.Point(93, 51);
            this.cmbToolName.Name = "cmbToolName";
            this.cmbToolName.Size = new System.Drawing.Size(264, 32);
            this.cmbToolName.TabIndex = 5;
            this.cmbToolName.SelectedIndexChanged += new System.EventHandler(this.cmbToolName_SelectedIndexChanged);
            // 
            // cmbProductCode
            // 
            this.cmbProductCode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbProductCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductCode.FormattingEnabled = true;
            this.cmbProductCode.Location = new System.Drawing.Point(93, 91);
            this.cmbProductCode.Name = "cmbProductCode";
            this.cmbProductCode.Size = new System.Drawing.Size(264, 32);
            this.cmbProductCode.TabIndex = 6;
            this.cmbProductCode.SelectedIndexChanged += new System.EventHandler(this.cmbProductCode_SelectedIndexChanged);
            // 
            // cmbSpec
            // 
            this.cmbSpec.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbSpec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpec.FormattingEnabled = true;
            this.cmbSpec.Location = new System.Drawing.Point(93, 129);
            this.cmbSpec.Name = "cmbSpec";
            this.cmbSpec.Size = new System.Drawing.Size(264, 32);
            this.cmbSpec.TabIndex = 7;
            this.cmbSpec.SelectedIndexChanged += new System.EventHandler(this.cmbSpec_SelectedIndexChanged);
            // 
            // tblBottom
            // 
            this.tblBottom.ColumnCount = 2;
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tblBottom.Controls.Add(this.btnSave, 1, 0);
            this.tblBottom.Controls.Add(this.lblBarcodePreview, 0, 0);
            this.tblBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBottom.Location = new System.Drawing.Point(3, 233);
            this.tblBottom.Name = "tblBottom";
            this.tblBottom.RowCount = 1;
            this.tblBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottom.Size = new System.Drawing.Size(972, 58);
            this.tblBottom.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(845, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(124, 52);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存记录";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblBarcodePreview
            // 
            this.lblBarcodePreview.BackColor = System.Drawing.Color.White;
            this.lblBarcodePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBarcodePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBarcodePreview.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarcodePreview.Location = new System.Drawing.Point(3, 0);
            this.lblBarcodePreview.Name = "lblBarcodePreview";
            this.lblBarcodePreview.Size = new System.Drawing.Size(836, 58);
            this.lblBarcodePreview.TabIndex = 2;
            this.lblBarcodePreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listTb
            // 
            this.listTb.ColumnCount = 1;
            this.listTb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.listTb.Controls.Add(this.dgvTodayRecords, 0, 0);
            this.listTb.Controls.Add(this.pnlListActions, 0, 1);
            this.listTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTb.Location = new System.Drawing.Point(3, 303);
            this.listTb.Name = "listTb";
            this.listTb.RowCount = 2;
            this.listTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.listTb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.listTb.Size = new System.Drawing.Size(978, 455);
            this.listTb.TabIndex = 1;
            // 
            // dgvTodayRecords
            // 
            this.dgvTodayRecords.AllowUserToAddRows = false;
            this.dgvTodayRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTodayRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTodayRecords.ContextMenuStrip = this.cmsRecords;
            this.dgvTodayRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTodayRecords.Location = new System.Drawing.Point(3, 3);
            this.dgvTodayRecords.Name = "dgvTodayRecords";
            this.dgvTodayRecords.ReadOnly = true;
            this.dgvTodayRecords.RowHeadersWidth = 62;
            this.dgvTodayRecords.RowTemplate.Height = 23;
            this.dgvTodayRecords.Size = new System.Drawing.Size(972, 409);
            this.dgvTodayRecords.TabIndex = 1;
            this.dgvTodayRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTodayRecords_CellContentClick);
            // 
            // cmsRecords
            // 
            this.cmsRecords.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsRecords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditQuantity,
            this.tsmiDelete});
            this.cmsRecords.Name = "cmsRecords";
            this.cmsRecords.Size = new System.Drawing.Size(153, 64);
            // 
            // tsmiEditQuantity
            // 
            this.tsmiEditQuantity.Name = "tsmiEditQuantity";
            this.tsmiEditQuantity.Size = new System.Drawing.Size(152, 30);
            this.tsmiEditQuantity.Text = "编辑数量";
            this.tsmiEditQuantity.Click += new System.EventHandler(this.tsmiEditQuantity_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(152, 30);
            this.tsmiDelete.Text = "删除";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // pnlListActions
            // 
            this.pnlListActions.Controls.Add(this.btnOpenSearch);
            this.pnlListActions.Controls.Add(this.btnOpenPreset);
            this.pnlListActions.Controls.Add(this.btnExport);
            this.pnlListActions.Controls.Add(this.btnSend);
            this.pnlListActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListActions.Location = new System.Drawing.Point(3, 418);
            this.pnlListActions.Name = "pnlListActions";
            this.pnlListActions.Size = new System.Drawing.Size(972, 34);
            this.pnlListActions.TabIndex = 2;
            // 
            // btnOpenSearch
            // 
            this.btnOpenSearch.Location = new System.Drawing.Point(180, 3);
            this.btnOpenSearch.Name = "btnOpenSearch";
            this.btnOpenSearch.Size = new System.Drawing.Size(150, 30);
            this.btnOpenSearch.TabIndex = 3;
            this.btnOpenSearch.Text = "查询历史数据";
            this.btnOpenSearch.Click += new System.EventHandler(this.btnOpenSearch_Click);
            // 
            // btnOpenPreset
            // 
            this.btnOpenPreset.Location = new System.Drawing.Point(12, 4);
            this.btnOpenPreset.Name = "btnOpenPreset";
            this.btnOpenPreset.Size = new System.Drawing.Size(150, 30);
            this.btnOpenPreset.TabIndex = 2;
            this.btnOpenPreset.Text = "打开预设数据界面";
            this.btnOpenPreset.Click += new System.EventHandler(this.btnOpenPreset_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(699, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 30);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "导出Excel";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(843, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 30);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "传输至打印端";
            // 
            // BarcodeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tblMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BarcodeEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日常条码录入";
            this.Load += new System.EventHandler(this.BarcodeEntryForm_Load);
            this.tblMain.ResumeLayout(false);
            this.tblInput.ResumeLayout(false);
            this.tblTop.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.baseInputTb.ResumeLayout(false);
            this.baseInputTb.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.pToolTb.ResumeLayout(false);
            this.pToolTb.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.specTb.ResumeLayout(false);
            this.specTb.PerformLayout();
            this.tblBottom.ResumeLayout(false);
            this.listTb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayRecords)).EndInit();
            this.cmsRecords.ResumeLayout(false);
            this.pnlListActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.TableLayoutPanel tblInput;
        private System.Windows.Forms.TableLayoutPanel tblTop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel baseInputTb;
        private AntdUI.Input txtRouteCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private AntdUI.Input txtQuantity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel pToolTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBrand;
        private System.Windows.Forms.ComboBox cmbCoating;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel specTb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbToolName;
        private System.Windows.Forms.ComboBox cmbProductCode;
        private System.Windows.Forms.ComboBox cmbSpec;
        private System.Windows.Forms.TableLayoutPanel tblBottom;
        private AntdUI.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel listTb;
        private System.Windows.Forms.DataGridView dgvTodayRecords;
        private System.Windows.Forms.ContextMenuStrip cmsRecords;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditQuantity;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.Panel pnlListActions;
        private AntdUI.Button btnOpenPreset;
        private AntdUI.Button btnExport;
        private AntdUI.Button btnSend;
        private System.Windows.Forms.Label lblBarcodePreview;
        private AntdUI.Button btnOpenSearch;
    }
}