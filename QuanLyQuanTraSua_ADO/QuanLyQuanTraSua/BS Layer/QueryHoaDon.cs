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
    class QueryHoaDon
    {

        Data db = null;
        DataRow[] filteredRows;
        DataSet ds, ds1;
        DataTable dt, dt1;
        public QueryHoaDon()
        {
            db = new Data();
            ds = db.ExecuteQueryDataSet("select * from KHACHHANG", CommandType.Text);
            dt = ds.Tables[0];

            ds1= db.ExecuteQueryDataSet("select * from Menu", CommandType.Text);
            dt1 = ds1.Tables[0];

        }
        public DataSet LayMenu()
        {
            return ds1;
        }
        public DataGridViewComboBoxColumn LoadComboBox(DataGridViewComboBoxColumn cbbMH)
        {
            DataSet dst;
            dst = LayMenu();
            DataTable dt = dst.Tables[0];
            cbbMH.DataSource = dt;
            cbbMH.DisplayMember = "TenMH";
            cbbMH.ValueMember = "MaMH";
            return cbbMH;
        }
        
        public bool CheckKhachHang(string sdt)
        {
            filteredRows = dt.Select("SDT LIKE '%" + sdt + "%'");
            if (filteredRows.Length == 0)
                return false;
            else
                return true;
        }

        public List<string> GetKhachInfor()
        {
            List<string> infor = new List<string>();
            string id = filteredRows[0]["MaKH"].ToString().Trim();
            string grade = filteredRows[0]["DiemTichLuy"].ToString().Trim();
            infor.Add(id);
            infor.Add(grade);
            return infor;
        }

        public bool ThemKhach(string name, string sdt, string diachi, ref string err, out List<string> infor)
        {
            int count = dt.Rows.Count;
            DataRow lastrow = dt.Rows[count - 1];
            string id = lastrow["MaKH"].ToString().Trim();
            string[] listcode = id.Split('H');
            int index = Int32.Parse(listcode[1]);
            string next_id = "KH";
            next_id = next_id + (index+1).ToString().PadLeft(4, '0');

            string sqlString = "Insert Into KHACHHANG Values(" + "'" + next_id + "',N'" + name + "','"+sdt+ "',N'"+ diachi+"',"+"0"+ ")";
            infor = new List<string>();
            infor.Add(next_id);
            infor.Add("0");
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);

        }

        public void GetGia(string idMH, out int cost, out int plus)
        {
            DataRow[] filter = dt1.Select("MaMH LIKE '%" + idMH + "%'");
            plus = Int32.Parse(filter[0]["DiemTichLuy"].ToString());
            cost= Int32.Parse(filter[0]["GiaTien"].ToString());
        }
        private string LocNgay(string datetime)
        {
            string[] list_time = datetime.Split(' ');
            string[] list_date = list_time[0].Split('/');
            DateTime date = new DateTime(Int32.Parse(list_date[2]), Int32.Parse(list_date[0]), Int32.Parse(list_date[1])) ;
            string result = date.ToString("yyyy-MM-dd");
            return result;
        }

        public float CheckCoupon(int cost, int point, out List<string> coupon_infor)
        {
            coupon_infor = new List<string>();
            DateTime today = DateTime.Now;

            DataSet dst = db.ExecuteQueryDataSet("select * from Coupon", CommandType.Text);
            DataTable dtt = dst.Tables[0];
            DataRow[] filter = dtt.Select();
            string start, end, now;
            now = today.ToString("yyyy-MM-dd");
            float discount = 0, rate, max = 0;
            int point_rate = 0;
            foreach (DataRow row in filter)
            {
                start = LocNgay(row["NgayBatDau"].ToString());
                end = LocNgay(row["NgayKetThuc"].ToString());
                rate = (float)row["MucGiam"];
                point_rate = (int)row["DiemApDung"];
                max = (float)row["GiamToiDa"];
                if (String.Compare(start, now) <= 0 && String.Compare(now, end) <= 0)
                    if (point >= point_rate)
                    {
                        discount = cost * rate;
                        if (discount > max)
                            discount = max;
                        coupon_infor.Add(row["MaCP"].ToString());
                        coupon_infor.Add(row["MoTa"].ToString());
                        break;
                    }
            }
            return discount;

        }

        public bool UpdateKhachHang(string maKH, int point, ref string err)
        {
            string sqlString = "Update KHACHHANG Set DiemTichLuy=" + point + " Where MaKH='" + maKH + "'";
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
        }

        public string LuuHoaDon(string maKH, string maNV, float total_cost, DateTime ngayXHD, string maCP, ref string err)
        {
            string sqlString;
            DataSet dst = db.ExecuteQueryDataSet("select * from HOADON", CommandType.Text);
            DataTable dtt = dst.Tables[0];
            int count = dtt.Rows.Count;
            DataRow lastrow = dtt.Rows[count - 1];
            string id = lastrow["MaHD"].ToString().Trim();
            string[] listcode = id.Split('D');
            int index = Int32.Parse(listcode[1]);
            string next_id = "HD";
            next_id = next_id + (index + 1).ToString().PadLeft(4, '0');
            if (maCP == "Null")
                sqlString = "Insert Into HOADON Values(" + "'" + next_id + "','" + maKH + "','" + maNV + "'," + total_cost.ToString() + ",'"+ ngayXHD.ToString("yyyy-MM-dd") + "',null)";
            else
                sqlString = "Insert Into HOADON Values(" + "'" + next_id + "','" + maKH + "','" + maNV + "'," + total_cost.ToString() + ",'"+ ngayXHD.ToString("yyyy-MM-dd") + "','" +maCP+ "')";
            db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);

            return next_id;

        }

        public void LuuChiTietHD(string maHD, DataGridView table_item, ref string err)
        {
            string sqlString = "";
            string maMH;
            int sl, point;
            float tien;
            foreach (DataGridViewRow row in table_item.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    maMH = row.Cells[0].Value.ToString().Trim();
                    sl = (int)row.Cells[1].Value;
                    point = (int)row.Cells[2].Value;
                    tien = (int)row.Cells[3].Value;
                    sqlString = "Insert Into CHITIETHOADON Values(" + "'" + maHD + "','" + maMH + "'," +sl.ToString() + "," +tien.ToString() + "," + point + ")";
                    db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err);
                }
            }
        }
    }
}
