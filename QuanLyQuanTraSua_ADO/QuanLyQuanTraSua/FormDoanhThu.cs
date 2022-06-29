using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanTraSua.BS_Layer;
using System.Data.SqlClient;

namespace QuanLyQuanTraSua
{
    public partial class FormDoanhThu : Form
    {

        DataTable dtDoanhThu = null;
        // Khai báo biến kiểm tra việc Thêm hay Sửa dữ liệu 
        bool Them;
        string err;
        QueryDoanhThu dbDT = new QueryDoanhThu();
        DateTime today = DateTime.Now;
        public FormDoanhThu()
        {
            InitializeComponent();
        }
       
        private void profit_view_btn_Click(object sender, EventArgs e)
        {
           
            profit_panel.Visible = true;
            this_month.Text = today.ToString("MM/yyyy");
            this_day.Text = today.ToString("dd/MM");
            num_year.Value = today.Year;
            CapNhatDoanhThu();
        }

        private void CapNhatDoanhThu()
        {
            float doanhthu = 0;
            int donhang = 0;
            dbDT.CapNhatDoanhThu(today);
            dbDT.CapNhatDoanhThuThang(today, out doanhthu, out donhang);
            now_order.Text = donhang.ToString();
            now_income.Text = doanhthu.ToString();
        }

        private void FormDoanhThu_Load(object sender, EventArgs e)
        {
            
        }

        private void show_btn_Click(object sender, EventArgs e)
        {
            this.DOANHTHUTableAdapter.FillBy(this.QuanLi.DOANHTHU, (int)num_year.Value);
            this.reportViewer1.RefreshReport();
        }
    }
}
