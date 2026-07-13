using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BarcodeMainApp.Forms;

namespace BarcodeMainApp
{
    public static class ThemeHelper
    {
        // ═══════════════════════════════════════════════════
        //  天蓝色配色方案
        // ═══════════════════════════════════════════════════
        private static readonly Color ColorBg          = Color.FromArgb(235, 245, 255); // 天蓝底
        private static readonly Color ColorCardBg      = Color.White;
        private static readonly Color ColorCardBorder  = Color.FromArgb(210, 225, 245);
        private static readonly Color ColorPrimary     = Color.FromArgb(0, 122, 255);   // 苹果蓝
        private static readonly Color ColorPrimaryDark = Color.FromArgb(0, 98, 210);
        private static readonly Color ColorPrimaryLight= Color.FromArgb(220, 238, 255);
        private static readonly Color ColorAccent      = Color.FromArgb(52, 180, 230);  // 亮蓝
        private static readonly Color ColorTitle       = Color.FromArgb(20, 30, 50);
        private static readonly Color ColorBody        = Color.FromArgb(70, 85, 105);
        private static readonly Color ColorMuted       = Color.FromArgb(140, 155, 175);
        private static readonly Color ColorRed         = Color.FromArgb(255, 80, 70);
        private static readonly Color ColorTableLine   = Color.FromArgb(228, 235, 245);
        private static readonly Color ColorInputBorder = Color.FromArgb(190, 208, 225);

        private static readonly Font FontCaption = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int GWL_EXSTYLE      = -20;
        private const int WS_EX_DROPSHADOW = 0x20000;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION        = 2;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

        public static void Apply(Form form)
        {
            form.BackColor = ColorBg;
            form.ForeColor = ColorBody;

            // 背景图 — 只对主窗体 SearchForm 生效
            if (form is BarcodeEntryForm || form is SearchForm)
            {
                string bg = "D:\\wwwww\\BarcodeMainApp-master\\beautify\\111.png";
                if (System.IO.File.Exists(bg))
                {
                    form.BackgroundImage = Image.FromFile(bg);
                    form.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            try { int c = 1; DwmSetWindowAttribute(form.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref c, sizeof(int)); } catch { }
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                int ex = GetWindowLong(form.Handle, GWL_EXSTYLE);
                SetWindowLong(form.Handle, GWL_EXSTYLE, ex | WS_EX_DROPSHADOW);
            }

            // 双缓冲减少闪烁
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(form, true);

            FadeIn(form);
            ConfigureControls(form.Controls);
        }

        private static void FadeIn(Form form)
        {
            form.Opacity = 0;
            form.Shown += (sender, args) =>
            {
                var t = new Timer { Interval = 12 };
                t.Tick += (ts, te) =>
                {
                    if (form.Opacity >= 0.95) { form.Opacity = 1; t.Stop(); t.Dispose(); }
                    else form.Opacity += 0.08;
                };
                t.Start();
            };
        }

        // ═══════════════════════════════════════════════════
        //  标题栏
        // ═══════════════════════════════════════════════════
        private static void AddTitleBar(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.None;

            var bar = new System.Windows.Forms.Panel
            {
                Height   = 42,
                Dock     = DockStyle.Top,
                BackColor = Color.White,
            };
            bar.Paint += (s, e) =>
            {
                using (var pen = new Pen(ColorCardBorder))
                    e.Graphics.DrawLine(pen, 0, bar.Height - 1, bar.Width, bar.Height - 1);
            };

            // 右侧关闭按钮
            var btnClose = new System.Windows.Forms.Button
            {
                Text      = "✕",
                Font      = new Font("Microsoft YaHei UI", 11F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = ColorMuted,
                Size      = new Size(36, 30),
                Location  = new Point(bar.Width - 40, 6),
                Anchor    = AnchorStyles.Top | AnchorStyles.Right,
                Cursor    = Cursors.Hand,
                TabStop   = false,
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 240, 250);
            btnClose.Click += (s, e) => form.Close();
            bar.Controls.Add(btnClose);

            var lbl = new System.Windows.Forms.Label
            {
                Text      = form.Text,
                Font      = new Font("Microsoft YaHei UI", 10.5F),
                ForeColor = ColorTitle,
                AutoSize  = false,
                Height    = 42,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            };
            bar.Controls.Add(lbl);

            MouseEventHandler drag = (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                { ReleaseCapture(); SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); }
            };
            bar.MouseDown += drag;
            lbl.MouseDown += drag;

            form.Controls.Add(bar);
            form.Controls.SetChildIndex(bar, 0);
        }

        private static void AddDot(System.Windows.Forms.Panel parent, int x, int y, Color color, EventHandler click)
        {
            var dot = new System.Windows.Forms.Panel
            {
                Location = new Point(x, y),
                Size     = new Size(12, 12),
                BackColor = color,
                Cursor   = Cursors.Hand,
                Anchor   = AnchorStyles.Top | AnchorStyles.Right,
            };
            dot.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(dot.BackColor))
                    g.FillEllipse(brush, 0, 0, 11, 11);
                if (_dotHover.Contains(dot))
                    using (var pen = new Pen(Color.White, 1.2f))
                    { g.DrawLine(pen, 3, 3, 8, 8); g.DrawLine(pen, 8, 3, 3, 8); }
            };
            dot.MouseEnter += (s, e) => { _dotHover.Add(dot); dot.BackColor = Color.FromArgb(255, 69, 58); dot.Invalidate(); };
            dot.MouseLeave += (s, e) => { _dotHover.Remove(dot); dot.BackColor = color; dot.Invalidate(); };
            dot.Click += click;
            parent.Controls.Add(dot);
        }
        private static readonly System.Collections.Generic.HashSet<System.Windows.Forms.Panel> _dotHover =
            new System.Collections.Generic.HashSet<System.Windows.Forms.Panel>();

        // ═══════════════════════════════════════════════════
        //  控件配置
        // ═══════════════════════════════════════════════════
        private static void ConfigureControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is AntdUI.Button btn)             { ConfigureButton(btn); }
                else if (c is AntdUI.Input inp)          { ConfigureInput(inp); }
                else if (c is System.Windows.Forms.GroupBox gb) { StyleGroupBox(gb); }
                else if (c is DataGridView dgv)          { StyleDataGridView(dgv); }
                else if (c is System.Windows.Forms.Label lbl) { lbl.ForeColor = Color.FromArgb(20, 40, 60); }
                else if (c is CheckBox chk)              { chk.ForeColor = ColorBody; chk.FlatStyle = FlatStyle.Flat; }
                else if (c is ComboBox cb)               { cb.FlatStyle = FlatStyle.Flat; cb.BackColor = Color.White; }
                else if (c is ListBox lb)                { StyleListBox(lb); }
                else if (c is TabControl tc)             { StyleTabControl(tc); }
                else if (c is TableLayoutPanel tlp)      { tlp.BackColor = Color.Transparent; }

                // 始终递归子控件
                if (c.HasChildren) { ConfigureControls(c.Controls); }
            }
        }

        private static void MakeTransparent(Control ctrl)
        {
            try
            {
                var setStyle = typeof(Control).GetMethod("SetStyle",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (setStyle != null)
                {
                    setStyle.Invoke(ctrl, new object[] { ControlStyles.SupportsTransparentBackColor, true });
                    setStyle.Invoke(ctrl, new object[] { ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true });
                }
                ctrl.BackColor = Color.Transparent;
            }
            catch { }
        }

        // ═══════════════════════════════════════════════════
        //  AntdUI 按钮
        // ═══════════════════════════════════════════════════
        private static void ConfigureButton(AntdUI.Button btn)
        {
            btn.Font       = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btn.Cursor     = Cursors.Hand;
            btn.Radius     = 8;
            btn.BorderWidth = 1;
            btn.WaveSize   = 3;

            if (btn.Text == "保存记录")
            {
                btn.Type = AntdUI.TTypeMini.Primary;
                btn.BackColor = Color.FromArgb(0, 122, 255);
                btn.ForeColor = Color.White;
                btn.WaveSize = 5;
            }
            else
            {
                btn.Type = AntdUI.TTypeMini.Default;
                btn.BackColor = Color.FromArgb(230, 242, 255);
                btn.ForeColor = Color.FromArgb(0, 80, 180);
            }
        }

        // ═══════════════════════════════════════════════════
        //  AntdUI 输入框
        // ═══════════════════════════════════════════════════
        private static void ConfigureInput(AntdUI.Input inp)
        {
            inp.Radius      = 10;
            inp.BorderWidth = 1.5F;
            inp.Font        = new Font("Microsoft YaHei UI", 10F);
            inp.BorderColor = Color.FromArgb(170, 190, 210);
            inp.BorderActive = Color.FromArgb(0, 120, 220);
            inp.BackColor = Color.White;

            if (inp.MaxLength == 3)
            {
                inp.TextAlign = HorizontalAlignment.Center;
            }
        }

        // ═══════════════════════════════════════════════════
        //  ListBox（自绘为圆角按钮样式）
        // ═══════════════════════════════════════════════════
        private static void StyleListBox(System.Windows.Forms.ListBox lb)
        {
            lb.BorderStyle = BorderStyle.None;
            lb.BackColor = Color.FromArgb(240, 245, 252);
            lb.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            lb.DrawMode = DrawMode.OwnerDrawFixed;
            lb.ItemHeight = 42;

            lb.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 3, e.Bounds.Width - 8, e.Bounds.Height - 6);
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                Color bg = selected ? Color.FromArgb(0, 120, 220) : Color.FromArgb(225, 238, 252);
                Color fg = selected ? Color.White : Color.FromArgb(0, 80, 160);

                using (var path = GetRoundRect(rect, 8))
                using (var brush = new SolidBrush(bg))
                {
                    g.FillPath(brush, path);
                }

                TextRenderer.DrawText(g, lb.Items[e.Index].ToString(), lb.Font, rect, fg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }

        // ═══════════════════════════════════════════════════
        //  TabControl（自绘标签页头）
        // ═══════════════════════════════════════════════════
        private static void StyleTabControl(System.Windows.Forms.TabControl tc)
        {
            tc.DrawMode = TabDrawMode.OwnerDrawFixed;
            tc.SizeMode = TabSizeMode.Fixed;
            tc.ItemSize = new Size(140, 40);
            tc.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            tc.Padding = new Point(12, 6);

            tc.DrawItem += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = e.Bounds;
                var tabRect = new Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6);
                bool selected = e.State == DrawItemState.Selected;

                Color bg = selected ? Color.FromArgb(0, 120, 220) : Color.FromArgb(225, 238, 252);
                Color fg = selected ? Color.White : Color.FromArgb(0, 80, 160);

                using (var path = GetRoundRect(tabRect, 8))
                using (var brush = new SolidBrush(bg))
                {
                    g.FillPath(brush, path);
                }

                TextRenderer.DrawText(g, tc.TabPages[e.Index].Text, tc.Font, tabRect, fg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }

        // ═══════════════════════════════════════════════════
        //  卡片（加大圆角 + 阴影）
        // ═══════════════════════════════════════════════════
        private static void StyleGroupBox(System.Windows.Forms.GroupBox gb)
        {
            gb.ForeColor = Color.FromArgb(0, 100, 200);
            gb.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);

            if (gb.FindForm() is BarcodeEntryForm)
            {
                // 主窗体卡片用 333.jpg
                gb.BackColor = Color.White;
                string cardBg = "D:\\wwwww\\BarcodeMainApp-master\\beautify\\333.jpg";
                if (System.IO.File.Exists(cardBg))
                {
                    gb.BackgroundImage = Image.FromFile(cardBg);
                    gb.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            else if (gb.FindForm() is SearchForm)
            {
                // 查询页面的卡片用 333.jpg
                gb.BackColor = Color.White;
                string dgvBg = "D:\\wwwww\\BarcodeMainApp-master\\beautify\\333.jpg";
                if (System.IO.File.Exists(dgvBg))
                {
                    gb.BackgroundImage = Image.FromFile(dgvBg);
                    gb.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            else
            {
                gb.BackColor = Color.White;
            }
        }

        // ═══════════════════════════════════════════════════
        //  表格
        // ═══════════════════════════════════════════════════
        private static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(220, 232, 248);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 238, 252);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 60, 100);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 32;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = ColorBody;

            // 查询页面的表格用 333.jpg
            if (dgv.FindForm() is SearchForm)
            {
                string dgvBg = "D:\\wwwww\\BarcodeMainApp-master\\beautify\\333.jpg";
                if (System.IO.File.Exists(dgvBg))
                {
                    dgv.BackgroundImage = Image.FromFile(dgvBg);
                    dgv.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            dgv.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = ColorPrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = ColorTitle;
            dgv.RowTemplate.Height = 30;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 252, 255);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = ColorBody;
        }

        private static GraphicsPath GetRoundRect(Rectangle rect, int r)
        {
            var path = new GraphicsPath();
            int d = r * 2;
            rect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
