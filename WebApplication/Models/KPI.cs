//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KPI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KPI()
        {
            this.KPIs = new HashSet<KPI>();
        }
    
        public int id { get; set; }
        public string MucTieu { get; set; }
        public int ChiTieu { get; set; }
        public string DonViTinh { get; set; }
        public int TyTrong { get; set; }
        public int KetQua { get; set; }
        public string GhiChu { get; set; }
        public string Email { get; set; }
        public int idKPI { get; set; }
        public string GhiChu2 { get; set; }
        public string Filename { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KPI> KPIs { get; set; }
        public virtual KPI KP1 { get; set; }
    }
}
