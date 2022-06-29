using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanTraSua.DB_Layer;
using System.Data;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryNhanVien
    {
        Data db = null;
        DataRow[] filteredRows;
        DataSet ds, ds1;
        DataTable dt, dt1;
        DateTime today = DateTime.Now;
        public QueryNhanVien()
        {
            db = new Data();
            ds = db.ExecuteQueryDataSet("select * from BANGLUONG", CommandType.Text);
            dt = ds.Tables[0];
        }

        public void CapNhatBangLuong()
        {
            int last_month, year;
            if (today.Month > 1)
            {
                last_month = today.Month - 1;
                year = today.Year;
            }
            else
            {
                last_month = 12;
                year = today.Year - 1;
            }

            DataRow[] row = dt.Select("Nam=" + year.ToString() + " and Thang=" + last_month.ToString());
            if (row.Length == 0)
            {
                CapNhatLuong(last_month, year);
            }
        }

        private void CapNhatLuong(int month, int year)
        {
            string err="";
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            DataSet temp1 = db.ExecuteQueryDataSet("select a.MaNV, sum(Luong) LuongThang from(select * from QUANLYLUONG where ThoiGian between'"+start.ToString("yyyy-MM-dd") +"' and '" +end.ToString("yyyy-MM-dd") + "') a Group by a.MaNV", CommandType.Text);
            DataTable temp2 = temp1.Tables[0];

            DataRow[] row = temp2.Select();
            string sqlString;
            foreach (DataRow r in row)
            {
                sqlString = "Insert Into BangLuong Values(" + year.ToString() + "," + month.ToString() + ",'" + (string)r["MaNV"] + "'," + (float)r["LuongThang"] + ")";
                db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
            }
        }

        public DataTable TinhLuongTam(int year, int month, int day)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, day);
            DataSet temp1 = db.ExecuteQueryDataSet("select a.MaNV, b.TenNV, sum(Luong) LuongThang from (select * from QUANLYLUONG where ThoiGian between'" + start.ToString("yyyy-MM-dd") + "' and '" + end.ToString("yyyy-MM-dd") + "') a inner join NhanVien b on a.MaNV=b.MaNV  Group by a.MaNV, b.TenNV", CommandType.Text);
            DataTable temp2 = temp1.Tables[0];
            return temp2;
        }

        public DataSet LayNhanVien()
        {
            return db.ExecuteQueryDataSet("select MaNV, TenNV, SDT, DiaChi, NgayNV, CMND from NHANVIEN where ConLam=1", CommandType.Text);
        }

        public bool ThemNhanVien(string MaNhanVien, string TenNhanVien, string sdt, string diaChi, string ngayNV, string cmnd, ref string err)
        {
            string sqlString = "Insert into NHANVIEN Values(" + "'" +
            MaNhanVien + "',N'" + TenNhanVien + "',N'" + sdt + "',N'" + diaChi + "',N'" + ngayNV + "',N'" + cmnd + "', 1)";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public bool XoaNhanVien(ref string err, string MaNhanVien)
        {
            string sqlString = "Update NHANVIEN Set ConLam=0 where MaNV like '%" + MaNhanVien + "%'";
            XoaDangNhap(MaNhanVien, ref err);
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        private bool XoaDangNhap(string MaNhanVien, ref string err)
        {
            string sqlString = "Delete from DangNhap2 where MaND like '%" + MaNhanVien + "%'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public bool CapNhatNhanVien(string MaNhanVien, string TenNhanVien, string sdt, string diaChi, string ngayNV, string cmnd, ref string err)
        {
            string sqlString = "Update dbo.NHANVIEN Set TenNV=N'" +
            TenNhanVien + "',SDT = N'" + sdt + "',DiaChi = N'" + diaChi + "',NgayNV = N'" + ngayNV + "',CMND = N'" + cmnd + "' Where MaNV like '%" + MaNhanVien + "%'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }

        public DataTable LocNhanVien(string text)
        {
            string sqlString = "select MaNV, TenNV, SDT, DiaChi, NgayNV, CMND from NHANVIEN where TenNV like '%" + text + "%' or MaNV like '%" + text + "%' or DiaChi like '%" + text + "%' or SDT like '%" + text + "%' or CMND like '%"+text+"%'";
            DataSet temp = db.ExecuteQueryDataSet(sqlString, CommandType.Text);
            return (temp.Tables[0]);
        }

        public string GetLastIndex()
        {
            DataSet set=db.ExecuteQueryDataSet("select * from NHANVIEN", CommandType.Text);
            DataTable tb = new DataTable();
            tb = set.Tables[0];
            DataRow[] filter = tb.Select();
            int count = filter.Length;
            DataRow lastrow = filter[count - 1];
            string id = lastrow["MaNV"].ToString().Trim();
            string[] listcode = id.Split('V');
            int index = Int32.Parse(listcode[1]);
            string next_id = "NV";
            next_id = next_id + (index + 1).ToString().PadLeft(3, '0');
            return next_id;
        }


    }
}
