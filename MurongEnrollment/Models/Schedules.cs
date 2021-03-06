//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MurongEnrollment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedules
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedules()
        {
            this.EnrolledSubjects = new HashSet<EnrolledSubjects>();
        }
    
        public string Id { get; set; }
        public string SchoolYearId { get; set; }
        public string SubjectId { get; set; }
        public string SectionId { get; set; }
        public string TeacherId { get; set; }
        public string Time { get; set; }
        public string Days { get; set; }
        public string Room { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrolledSubjects> EnrolledSubjects { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Sections Sections { get; set; }
        public virtual Subjects Subjects { get; set; }
        public virtual Teachers Teachers { get; set; }
    }
}
