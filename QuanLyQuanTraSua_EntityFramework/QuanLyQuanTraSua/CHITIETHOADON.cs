//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyQuanTraSua
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETHOADON
    {
        public string MaHD { get; set; }
        public string MaMH { get; set; }
        public int SoLuong { get; set; }
        public Nullable<float> Tien { get; set; }
        public Nullable<int> DiemTichLuy { get; set; }
    
        public virtual HOADON HOADON { get; set; }
        public virtual MENU MENU { get; set; }
    }
}