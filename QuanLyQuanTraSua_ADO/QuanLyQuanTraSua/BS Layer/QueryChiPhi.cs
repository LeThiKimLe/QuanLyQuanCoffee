using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanTraSua.DB_Layer;
using System.Data;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryChiPhi
    {
        Data db = null;
        public QueryChiPhi()
        {
            db = new Data();
        }
        public DataSet LayChiPhi()
        {
            return db.ExecuteQueryDataSet("select * from CHI", CommandType.Text);
        }

        public bool CapNhatChiPhi(string Year, string Month, string LuongNV, string PNL, string tiendien, string tiennuoc, string pvs, string tong, ref string err)
        {
            string sqlString = "Update CHI Set LuongNV='" + LuongNV +
            "',PhiNguyenLieu='" + PNL + "',TienDien='" + tiendien + "',TienNuoc='" + tiennuoc + "',PhiVeSinh='" + pvs + "',Tong='" + tong + "' Where Nam='" + Year + "' and Thang='" + Month + "'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public bool ThemChiPhi(string Year, string Month, string LuongNV, string PNL, string tiendien, string tiennuoc, string pvs, string tong, ref string err)
        {
            string sqlString = "Insert Into CHI Values(" + "'" +
            Year + "','" +
            Month + "','" + LuongNV + "','" + PNL + "','" + tiendien + "','" + tiennuoc + "','" + pvs + "','" + tong + "')";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public DataSet LayThongTin(string year)
        {
            string strsql = "Select * from CHI where Nam='" + year + "'";
            return db.ExecuteQueryDataSet(strsql, CommandType.Text);
        }
        public DataSet Kiemtra(string year, string month)
        {
            string sqlstring = "Select * From CHI Where Nam='" + year + "' and Thang='" + month + "'";
            return db.ExecuteQueryDataSet(sqlstring, CommandType.Text);
        }

        public string CapNhatChiPhiHienTai(DateTime today)
        {
            string sqlstring = "Select Sum(Luong) as LuongTam From QUANLYLUONG Where YEAR(ThoiGian)=" + today.Year.ToString() + "and MONTH(ThoiGian)=" + today.Month.ToString();
            DataTable tb= (db.ExecuteQueryDataSet(sqlstring, CommandType.Text)).Tables[0];
            if (tb.Rows.Count!=0)
                return tb.Rows[0]["LuongTam"].ToString();
            return "0";
        }

    }
}
