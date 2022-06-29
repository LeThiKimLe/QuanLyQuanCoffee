using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLyQuanTraSua.BS_Layer
{
    class QueryLoiNhuan
    {

        public DataTable LayLoiNhuan()
        {
            
            QUANLYTRASUAEntities qlbhEntity = new QUANLYTRASUAEntities();
            var tsb = from p in qlbhEntity.LOINHUANs
                      select p;

            DataTable tb = new DataTable();
            tb.Columns.Add("Nam");
            tb.Columns.Add("Thang");
            tb.Columns.Add("DoanhThu");
            tb.Columns.Add("ChiPhi");
            tb.Columns.Add("LoiNhuan");

            foreach (var p in tsb)
                tb.Rows.Add(p.Nam, p.Thang, p.DoanhThu, p.ChiPhi, p.LoiNhuan1);
            return tb;
        }
        public bool ThemLoiNhuan(string Year, string Month, string doanhthu, string chiphi, string loinhuan, ref string err)
        {
            QUANLYTRASUAEntities qlbhEntity = new QUANLYTRASUAEntities();
            int year = Int32.Parse(Year);
            int thang = Int32.Parse(Month);

            LOINHUAN ln = new LOINHUAN();
            ln.Nam = Int32.Parse(Year);
            ln.Thang = Int32.Parse(Month);
            ln.DoanhThu = (float?)double.Parse(doanhthu);
            ln.ChiPhi = (float?)double.Parse(chiphi);
            ln.LoiNhuan1= (float?)double.Parse(loinhuan);

            qlbhEntity.LOINHUANs.Add(ln);
            qlbhEntity.SaveChanges();

            return true;
        }

        public bool CapNhatLoiNhuan(string Year, string Month, string doanhthu, string chiphi, string loinhuan, ref string err)
        {
            int year = Int32.Parse(Year);
            int thang = Int32.Parse(Month);

            QUANLYTRASUAEntities qlbhEntity = new QUANLYTRASUAEntities();
            var tsb = (from p in qlbhEntity.LOINHUANs
                       where p.Nam == year && p.Thang == thang
                       select p).SingleOrDefault();

            if (tsb != null)
            {
                tsb.DoanhThu = (float)(Double.Parse(doanhthu));
                tsb.ChiPhi = (float)(Double.Parse(chiphi));
                tsb.LoiNhuan1 = (float)(Double.Parse(loinhuan));
                qlbhEntity.SaveChanges();
            }
            return true;
        }
        public DataTable LayLoiNhuanTuDTCP()
        {

            QUANLYTRASUAEntities qlbhEntity = new QUANLYTRASUAEntities();
            var tsb = from p in qlbhEntity.DOANHTHUs
                       join sa in qlbhEntity.CHIs on new { Nam = p.Nam, Thang = p.Thang } equals new { Nam = sa.Nam, Thang = sa.Thang}
                       select new
                       {
                           Nam = p.Nam,
                           Thang = p.Thang,
                           DoanhThu = p.DoanhThu1,
                           ChiPhi = sa.Tong,
                           LoiNhuan = p.DoanhThu1 - sa.Tong
                       };

            DataTable tb = new DataTable();
            tb.Columns.Add("Nam");
            tb.Columns.Add("Thang");
            tb.Columns.Add("DoanhThu");
            tb.Columns.Add("ChiPhi");
            tb.Columns.Add("LoiNhuan");

            foreach (var p in tsb)
                tb.Rows.Add(p.Nam, p.Thang, p.DoanhThu, p.ChiPhi, p.LoiNhuan);
            return tb;
        }

        public DataTable Kiemtra(string year, string month)
        {
            int Year = Int32.Parse(year);
            int Thang = Int32.Parse(month);

            QUANLYTRASUAEntities qlbhEntity = new QUANLYTRASUAEntities();
            var tsb = (from p in qlbhEntity.LOINHUANs
                       where p.Nam == Year && p.Thang == Thang
                       select p).SingleOrDefault();

           
            DataTable tb = new DataTable();
            tb.Columns.Add("Nam");
            tb.Columns.Add("Thang");
            tb.Columns.Add("DoanhThu");
            tb.Columns.Add("ChiPhi");
            tb.Columns.Add("LoiNhuan");
                
            tb.Rows.Add(tsb.Nam, tsb.Thang, tsb.DoanhThu, tsb.ChiPhi, tsb.LoiNhuan1);
            return tb;
            
          
        }

    }
}
