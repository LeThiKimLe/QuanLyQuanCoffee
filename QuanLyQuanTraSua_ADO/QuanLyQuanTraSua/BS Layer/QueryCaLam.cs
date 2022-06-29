using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanLyQuanTraSua.DB_Layer;


namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryCaLam
    {
        Data db = null;
        DataSet ds;
        DataTable dt;
        public QueryCaLam()
        {
            db = new Data();
            ds = db.ExecuteQueryDataSet("select * from QUANLYLUONG", CommandType.Text);
            dt = ds.Tables[0];
        }

        public bool Regis_Shift(string maNV, string maCa, string tgian, ref string err)
        {
            string sqlString;
            sqlString = "Select * from QUANLYLUONG Where Thoigian='" + tgian + "' and MaNV like'%" + maNV + "%' and MaCa like'%" + maCa + "%'";
            DataSet x = db.ExecuteQueryDataSet(sqlString, CommandType.Text);
            DataTable y = x.Tables[0];
            if (y.Rows.Count != 0)
                return false;

            sqlString = "Insert Into QUANLYLUONG Values(" + "'" + tgian + "','" + maNV + "','" + maCa + "',0,0)";
            db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
            return true;
        }

        private double TienLuongca(string maCa)
        {
            DataSet temp = db.ExecuteQueryDataSet("select * from CaLam where MaCa like '%"+maCa+"%'", CommandType.Text);
            DataTable temp2 = temp.Tables[0];
            DataRow row = temp2.Rows[0];
            return (double)row["LuongTheoGio"];
        }

        public Dictionary<string, List<TimeSpan>> ThoiGianLam()
        {
            Dictionary<string, List<TimeSpan>> list_time_ca= new Dictionary<string, List<TimeSpan>>();
            DataSet temp = db.ExecuteQueryDataSet("select * from CaLam", CommandType.Text);
            DataTable temp2 = temp.Tables[0];
            DataRow [] row = temp2.Select();

            string ca;
            TimeSpan start, end;
            List<TimeSpan> tempt= new List<TimeSpan>();

            foreach(DataRow r in row)
            {
                tempt = new List<TimeSpan>();

                start = (TimeSpan)r["ThoiGianBatDau"];
                end = (TimeSpan)r["ThoiGianKetThuc"];

                tempt.Add(start);
                tempt.Add(end);
                ca = ((string)r["MaCa"]).Trim();
                list_time_ca[ca] = tempt;

            }
            return list_time_ca;
        }

        public Dictionary<string, int> Max_nv()
        {
            Dictionary<string, int> list_max_ca = new Dictionary<string, int>();
            DataSet temp = db.ExecuteQueryDataSet("select * from CaLam", CommandType.Text);
            DataTable temp2 = temp.Tables[0];
            DataRow[] row = temp2.Select();
            foreach (DataRow r in row)
                list_max_ca.Add(((string)r["MaCa"]).Trim(), (int)r["NhanVienToiDa"]);
            return list_max_ca;
        }

        public bool Finish_work(string maNV, string tgian, string maCa, int rate, ref string err)
        {
            string sqlString;
            sqlString = "Select * from QUANLYLUONG Where Thoigian='" + tgian + "' and MaNV like'%" + maNV + "%' and MaCa like'%" + maCa + "%'";
            DataSet x = db.ExecuteQueryDataSet(sqlString, CommandType.Text);
            DataTable y = x.Tables[0];
            if (y.Rows.Count == 0)
                return false;
            double luong = 4 * TienLuongca(maCa) * rate;
            sqlString = "Update QUANLYLUONG Set MucDoHoanThanh=" + rate +", Luong="+luong+" Where Thoigian='" + tgian + "' and MaNV like'%"+maNV+"%' and MaCa like'%"+maCa+"%'";
            return(db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err));
        }
        private DataTable GetRegisList(DateTime start, DateTime end)
        {
            DataSet temp = db.ExecuteQueryDataSet("select * from QUANLYLUONG where ThoiGian between '" + start.ToString("yyyy-MM-dd") + "' and '" + end.ToString("yyyy-MM-dd") + "'", CommandType.Text);
            DataTable temp_table = temp.Tables[0];
            return temp_table;
        }

        private string Get_shift_code(DataRow row)
        {
            return ((string)row["MaCa"]).Trim() + "-" + ((DateTime)row["ThoiGian"]).ToString("dd/MM");
        }

        private string GetName(string MaNV)
        {
            DataSet temp = db.ExecuteQueryDataSet("select * from NHANVIEN where MaNV like'%"+MaNV+"%'", CommandType.Text);
            DataTable dt = temp.Tables[0];
            DataRow r = dt.Rows[0];
            string name = (string)r["TenNV"];
            string[] list_name = name.Split(' ');
            return list_name[list_name.Length - 1];
        }

        public List<List<string>> LoadTimeTable(DateTime start, DateTime end)
        {
            List<List<string>> list_ca=new List<List<string>>();
            DataTable tb = GetRegisList(start, end);
            DataRow[] list_row = tb.Select();
            List<string> temp = new List<string>();
            foreach (DataRow row in list_row)
            {
                temp = new List<string>();
                temp.Add(Get_shift_code(row));
                temp.Add((string)row["MaNV"]);
                temp.Add(GetName((string)row["MaNV"]));
                list_ca.Add(temp);
            }
            return list_ca;
        }

    }
}
