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
    
    public partial class DOANHTHU
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public Nullable<float> DoanhThu1 { get; set; }
        public Nullable<int> SoDonHang { get; set; }
    
        public virtual LOINHUAN LOINHUAN { get; set; }
    }
}
