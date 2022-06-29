using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanTraSua.DB_Layer;
using System.Data;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryDoanhThu
    {
        Data db = null;
        public QueryDoanhThu()
        {
            db = new Data();
        }
      
        public void LayDoanhThuTudonHang(string year, string month, out float doanhthu, out int sdh, string day="32")
        {
            doanhthu = 0;
            sdh = 0;
            DataSet ds=db.ExecuteQueryDataSet("select SUM(ThanhTien) AS DoanhThu, COUNT(MAHD) AS SoDonHang from HOADON where YEAR(NgayXuatHD)=" + year + "AND MONTH(NgayXuatHD) = " + month + "AND DAY(NgayXuatHD) <" + day, CommandType.Text);
            DataTable tb = ds.Tables[0];
            if (tb.Rows.Count!=0)
            {
                doanhthu = (float)(double)(tb.Rows[0]["DoanhThu"]);
                sdh = (int)(tb.Rows[0]["SoDonHang"]);
            }
        }

       
        public bool ThemDoanhThu(string Year, string Month, string doanhthu, string sohoadon, ref string err)
        {
            string sqlString = "Insert Into DOANHTHU Values(" + "'" +
            Year + "','" +
            Month + "','" + doanhthu + "','" + sohoadon + "')";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }

        public void CapNhatDoanhThu(DateTime today)
        {
            int year = today.Year;
            int month = today.Month;
            int last_month = 1;
            float doanhthu = 0;
            int donhang = 0;
            string err="";
            if (month > 1)
                last_month = month - 1;
            else
            {
                last_month = 12;
                year = year - 1;
            }
            bool check = Kiemtra(year.ToString(), last_month.ToString());
            if (check == true)
                return;
            else
            {
                LayDoanhThuTudonHang(year.ToString(), last_month.ToString(), out doanhthu, out donhang);
                ThemDoanhThu(year.ToString(), last_month.ToString(), doanhthu.ToString(), donhang.ToString(), ref err);
            }
        }

        public void CapNhatDoanhThuThang(DateTime today, out float doanhthu, out int donhang)
        {
            int year = today.Year;
            int month = today.Month;
            int day = today.Day;
            int last_month;
            doanhthu = 0;
            donhang = 0;
            LayDoanhThuTudonHang(year.ToString(), month.ToString(), out doanhthu, out donhang);
        }

        public bool Kiemtra(string year, string month)
        {
            string sqlstring = "Select * From DOANHTHU Where Nam='" + year + "' and Thang='" + month + "'";
            DataSet ds= db.ExecuteQueryDataSet(sqlstring, CommandType.Text);
            DataTable tb = ds.Tables[0];
            if (tb.Rows.Count == 0)
                return false;
            return true;
        }


    }
}
