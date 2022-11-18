//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuidanceConsultancy.Models.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mst_School
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mst_School()
        {
            this.Mst_Student = new HashSet<Mst_Student>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> SchoolTypeId { get; set; }
        public Nullable<int> SchoolMediumId { get; set; }
    
        public virtual Mst_SchoolType Mst_SchoolType { get; set; }
        public virtual Mst_SchoolMedium Mst_SchoolMedium { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mst_Student> Mst_Student { get; set; }
    }
}