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
    
    public partial class sopUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sopUser()
        {
            this.sopKayit = new HashSet<sopKayit>();
            this.sopOnay = new HashSet<sopOnay>();
        }
    
        public int user_id { get; set; }
        public string kullaniciAdi { get; set; }
        public string adSoyad { get; set; }
        public string tcKimlik { get; set; }
        public string sicilNo { get; set; }
        public string sifre { get; set; }
        public string email { get; set; }
        public string unvan { get; set; }
        public Nullable<int> yetkiID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sopKayit> sopKayit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sopOnay> sopOnay { get; set; }
    }
}
