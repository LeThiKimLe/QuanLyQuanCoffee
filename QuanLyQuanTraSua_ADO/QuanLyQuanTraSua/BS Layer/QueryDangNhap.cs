using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using QuanLyQuanTraSua.DB_Layer;

namespace QuanLyQuanTraSua
{
    class QueryDangNhap
    {
        Data db = null;
        public QueryDangNhap()
        {
            db = new Data();
        }

        public bool checkTaiKhoan_QL(string username, string pass, out string userID, out string name)
        {
            userID = "";
            name = "";
            DataSet ds;
            ds = db.ExecuteQueryDataSet("select * from DangNhap a, QuanLy b where a.MaND=b.MaQL", CommandType.Text);
            DataTable dt = ds.Tables[0];
            DataRow[] fillteredRow = dt.Select("TenDangNhap LIKE '%" + username + "'and MatKhau LIKE '%" + pass + "%'");

            if (fillteredRow.Length == 0)
            {
                
                return false;
            }
            userID = (string)fillteredRow[0]["MaND"];
            name = (string)fillteredRow[0]["TenQL"];
            return true;
        }

        

        public bool checkTaiKhoan_NV(string username, string pass, out string userID, out string name)
        {
            userID = "";
            name = "";
            DataSet ds;
            ds = db.ExecuteQueryDataSet("select * from DangNhap2 a, NhanVien b where a.MaND=b.MaNV", CommandType.Text);
            DataTable dt = ds.Tables[0];
            DataRow[] fillteredRow = dt.Select("TenDangNhap LIKE '%" + username + "'and MatKhau LIKE '%" + pass + "%'");

            if (fillteredRow.Length == 0)
            {
                return false;
            }
            userID = (string)fillteredRow[0]["MaND"];
            name = (string)fillteredRow[0]["TenNV"];
            return true;
        }

    }
}
