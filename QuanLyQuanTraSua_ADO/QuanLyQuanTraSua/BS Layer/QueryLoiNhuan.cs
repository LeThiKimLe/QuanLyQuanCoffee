using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanTraSua.DB_Layer;
using System.Data;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryLoiNhuan
    {

        Data db = null;
        public QueryLoiNhuan()
        {
            db = new Data();
        }

        public DataSet LayLoiNhuan()
        {
            return db.ExecuteQueryDataSet("select * from LOINHUAN", CommandType.Text);
        }
        public bool ThemLoiNhuan(string Year, string Month, string doanhthu, string chiphi, string loinhuan, ref string err)
        {
            string sqlString = "Insert Into LOINHUAN Values(" + "'" +
            Year + "','" +
            Month + "','" + doanhthu + "','" + chiphi + "','" + loinhuan + "')";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }

        public bool CapNhatLoiNhuan(string Year, string Month, string doanhthu, string chiphi, string loinhuan, ref string err)
        {
            string sqlString = "Update LOINHUAN Set DoanhThu=N'" + doanhthu +
            "',CHIPHI='" + chiphi + "',LoiNhuan='" + loinhuan + "' Where Nam='" + Year + "' and Thang='" + Month + "'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }
        public DataSet LayLoiNhuanTuDTCP()
        {
            string strsql = "Select DOANHTHU.NAM, DOANHTHU.THANG,DOANHTHU.DoanhThu, CHI.Tong as ChiPhi, DOANHTHU.DoanhThu-CHI.Tong AS LoiNhuan FROM DOANHTHU, CHI WHERE DOANHTHU.Nam= CHI.Nam AND DOANHTHU.Thang= CHI.Thang";
            return db.ExecuteQueryDataSet(strsql, CommandType.Text);
        }

        public DataSet Kiemtra(string year, string month)
        {
            string sqlstring = "Select * From LOINHUAN Where Nam='" + year + "' and Thang='" + month + "'";
            return db.ExecuteQueryDataSet(sqlstring, CommandType.Text);
        }

    }
}
