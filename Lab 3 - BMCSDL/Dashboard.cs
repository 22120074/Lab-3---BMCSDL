﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_3___BMCSDL
{
    public partial class Dashboard : Form
    {
        private Panel headerPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Label lblAppTitle;
        private Label lblUser;
        private Button btnLogout;
        private Button btnDashboard;
        private Button btnQuanLyLop;
        private Button btnQuanLySV;
        private Button btnNhapDiem;
        private Button btnDataNV;
        private string _manv;
        private string _pass;

        public Dashboard(string manv, string pass)
        {
            InitializeComponent();
            _manv = manv;
            _pass = pass;
            InitializeComponent_Dashboard();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // Lấy kích thước màn hình hiện tại
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Thiết lập kích thước form
            this.Width = 1500;
            this.Height = 1000;

            // Tính toán vị trí để căn giữa
            int x = (screen.Width - this.Width) / 2;
            int y = (screen.Height - this.Height) / 2;

            this.Location = new Point(x, y);

            // Hiển thị trang mặc định
            ShowContent(new UcDashboard());
        }

        private void InitializeComponent_Dashboard()
        {
            // --- Kích thước cố định của form ---
            this.Text = "Dashboard";
            this.ClientSize = new Size(1500, 1000);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // --- Màu chính ---
            Color primaryColor = Color.FromArgb(0x2D, 0x8C, 0xFF);

            // --- Thanh Header ---
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = primaryColor
            };
            this.Controls.Add(headerPanel);

            // --- Tiêu đề ứng dụng ---
            lblAppTitle = new Label
            {
                Text = "QLSV Dashboard",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            headerPanel.Controls.Add(lblAppTitle);

            // --- Nhãn người dùng ---
            lblUser = new Label
            {
                Text = $"Xin chào, [ {_manv} ]",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            headerPanel.Controls.Add(lblUser);
            lblUser.Location = new Point(headerPanel.Width - 120 - lblUser.Width, 20);

            // --- Nút đăng xuất ---
            btnLogout = new Button
            {
                Text = "Đăng xuất",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = primaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(headerPanel.Width - 120, 15)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            headerPanel.Controls.Add(btnLogout);
            btnLogout.Click += BtnLogout_Click;

            // Khoảng cách giữa header và sidebar
            int verticalGap = 30;
            int horizontalGap = 30;

            // Thông số nút và margin
            int btnWidth = 200;
            int btnHeight = 40;
            int btnCount = 5;                           // 5 nút
            int btnSpacing = 10;                        // khoảng cách giữa các nút
            int marginTop = 20;                         // khoảng cách từ đỉnh panel đến nút đầu
            int marginBottom = 20;                      // khoảng cách từ nút cuối đến đáy panel

            int sidebarX = horizontalGap;
            int sidebarY = headerPanel.Height + verticalGap;
            int sidebarWidth = btnWidth + 20;           // đệm 10px trái + 10px phải

            // Tính chiều cao vừa đủ cho 4 nút
            int sidebarHeight = marginTop + btnCount * btnHeight + (btnCount - 1) * btnSpacing + marginBottom;

            // Tạo sidebar panel
            sidebarPanel = new Panel
            {
                BackColor = primaryColor,
                Location = new Point(sidebarX, sidebarY),
                Size = new Size(sidebarWidth, sidebarHeight)
            };
            this.Controls.Add(sidebarPanel);

            // --- Các nút trên sidebar ---
            int startY = marginTop;                     // bắt đầu từ marginTop

            btnDashboard = CreateSidebarButton("Dashboard", 10, startY);
            sidebarPanel.Controls.Add(btnDashboard);
            btnDashboard.Click += (s, e) => ShowContent(new UcDashboard());

            btnQuanLyLop = CreateSidebarButton("Quản lý Lớp", 10, startY + (btnHeight + btnSpacing) * 2);
            sidebarPanel.Controls.Add(btnQuanLyLop);
            btnQuanLyLop.Click += (s, e) => ShowContent(new UcQuanLyLop());

            btnQuanLySV = CreateSidebarButton("Quản lý SV", 10, startY + (btnHeight + btnSpacing) * 3);
            sidebarPanel.Controls.Add(btnQuanLySV);
            btnQuanLySV.Click += (s, e) => ShowContent(new UcQuanLySV(_manv));

            btnNhapDiem = CreateSidebarButton("Nhập Điểm", 10, startY + (btnHeight + btnSpacing) * 4);
            sidebarPanel.Controls.Add(btnNhapDiem);
            btnNhapDiem.Click += (s, e) => ShowContent(new UcNhapDiem(_manv, _pass));

            btnDataNV = CreateSidebarButton("Dữ liệu NV", 10, startY + (btnHeight + btnSpacing) * 1);
            sidebarPanel.Controls.Add(btnDataNV);
            btnDataNV.Click += (s, e) => ShowContent(new UcDataNhanVien(_manv));

            // --- Khung nội dung chính (Content) --- 
            int contentX = sidebarPanel.Right + horizontalGap;
            int contentY = sidebarY;
            int contentWidth = this.ClientSize.Width - contentX;
            int contentHeight = 900 - headerPanel.Height - verticalGap;

            contentPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(contentX, contentY),
                Size = new Size(contentWidth, contentHeight)
            };
            this.Controls.Add(contentPanel);
        }

        // --- Tạo nút sidebar ---
        private Button CreateSidebarButton(string text, int x, int y)
        {
            Color primaryColor = Color.FromArgb(0x2D, 0x8C, 0xFF);
            var btn = new Button()
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = primaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Location = new Point(x, y)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            return btn;
        }

        // --- Hiển thị nội dung mới --- 
        private void ShowContent(UserControl control)
        {
            // Xóa nội dung cũ
            contentPanel.Controls.Clear();

            // Dock full cho control mới
            control.Dock = DockStyle.Fill;

            // Thêm vào content panel
            contentPanel.Controls.Add(control);
        }

        // --- Đăng xuất ---
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            this.Hide(); 
            loginForm.ShowDialog();
            this.Close(); 
        }

    }
}
