using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab_3___BMCSDL
{
    public partial class UcDataNhanVien : UserControl
    {
        private DataGridView dgvNV;
        private string connectionString = @"Server=LAPTOP-RBM16H2U\MSSQLSER2022;Database=QLSVNhom;Trusted_Connection=True;";
        private string _manv;

        public UcDataNhanVien(string manv)
        {
            InitializeComponent();
            InitializeGrid();
            _manv = manv;
            LoadThongTinNhanVien();
        }

        private void InitializeGrid()
        {
            this.Dock = DockStyle.Fill;                 // Phủ hết phần content 
            this.BackColor = Color.White;

            Panel containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 50, 0),     // Tạo khoảng cách phải
                BackColor = Color.White
            };

            dgvNV = new DataGridView
            {
                Dock = DockStyle.Top,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ScrollBars = ScrollBars.Vertical        // Cho cuộn dọc nếu dữ liệu dài
            };

            containerPanel.Controls.Add(dgvNV);
            this.Controls.Add(containerPanel);
        }

        private void LoadThongTinNhanVien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SP_QUANLY_NHANVIEN", conn); // Tên stored procedure lấy danh sách nhân viên
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MANV", _manv);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvNV.DataSource = table;

                    // Tính và cập nhật chiều cao tự động dựa trên số dòng
                    int newHeight = TinhChieuCaoDataGridView(dgvNV);
                    int maxHeight = 900 - 60 - 30;
                    dgvNV.Height = Math.Min(newHeight, maxHeight);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int TinhChieuCaoDataGridView(DataGridView dgv)
        {
            int totalHeight = dgv.ColumnHeadersHeight;  // Chiều cao phần header

            foreach (DataGridViewRow row in dgv.Rows)
            {
                totalHeight += row.Height;
            }
            totalHeight += 10;                          // padding nhỏ bên dưới

            return totalHeight;
        }

    }
}
