//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SOP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sopOnay
    {
        public int onayID { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> sopID { get; set; }
        public Nullable<bool> durum { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
    
        public virtual sopKayit sopKayit { get; set; }
        public virtual sopUser sopUser { get; set; }
    }
}
