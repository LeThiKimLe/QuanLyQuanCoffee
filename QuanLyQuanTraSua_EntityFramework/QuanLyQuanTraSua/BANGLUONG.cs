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
    
    public partial class BANGLUONG
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public string MaNV { get; set; }
        public Nullable<float> TienLuong { get; set; }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}