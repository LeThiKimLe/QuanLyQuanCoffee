using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QuanLyQuanTraSua.DB_Layer;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryKhachHang
    {
        Data db = null;
        public QueryKhachHang()
        {
            db = new Data();
        }
        public DataSet LayKhachHang()
        {
            return db.ExecuteQueryDataSet("select * from KHACHHANG", CommandType.Text);
        }

        public bool CapNhatKhachHang(string MaKhachHang, string TenKH, string DiaChi, string DienThoai, ref string err)
        {
            string sqlString = "Update KHACHHANG Set TenKH=N'" + TenKH +
            "',SDT='" + DienThoai + "',DiaChi=N'" + DiaChi + "' Where MaKH='" + MaKhachHang + "'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public DataSet LayThongTin(string makh)
        {

            string strsql = "Select * from KHACHHANG where MaKH='" + makh + "'";
            return db.ExecuteQueryDataSet(strsql, CommandType.Text);
        }

        public DataTable LocKhachHang(string text)
        {
            string strsql = "Select * from KHACHHANG where TenKH like '%" + text + "%' or MaKH like '%"+ text + "%' or DiaChi like '%" + text + "%' or SDT like '%" +text + "%'";
            DataTable tb = (db.ExecuteQueryDataSet(strsql, CommandType.Text)).Tables[0];
            return tb;

        }

    }

}

