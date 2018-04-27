using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MurongEnrollment.Models
{
    public partial class Students
    {
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string GradeSection { get { return this.Enrollments.Where(m => m.SchoolYears.isActive == true).FirstOrDefault()?.Sections.GradeLevels.GradeLevel + " " + this.Enrollments.Where(m => m.SchoolYears.isActive == true).FirstOrDefault()?.Sections.SectionName; } }
        public Enrollments Enrollment { get; set; }
        public IEnumerable<object> GenderList
        {
            get
            {
                return Enum.GetValues(typeof(Genders)).Cast<Genders>().Select(x => new { Id = x, Name = x.ToString() });
            }
        }

        public IEnumerable<object> StudentStatusList
        {
            get
            {
                return Enum.GetValues(typeof(StudentStatus)).Cast<StudentStatus>().Select(x => new { Id = x, Name = x.ToString() });
            }
        }
        public decimal? TotalBalance
        {
            get
            {
                var ret = (this.Enrollments.Where(m => m.SchoolYears.isActive == true).FirstOrDefault()?.TotalBalance ?? 0.0M);
                return ret;
            }

        }
        public string BalanceStatus
        {
            get
            {
                var totalBalance = TotalBalance; ;
                return totalBalance <= 0.0M ? "Fully Paid" : totalBalance?.ToString("N2");
            }

        }
        public string Status
        {
            get
            {
                if (this.Enrollments.Where(e => e.SchoolYearId.Contains(new UnitOfWork().SchoolYearRepo.Fetch(m => m.isActive == true).FirstOrDefault().Id)).Count() > 0)
                    return "Enrolled ";
                else
                    return "Enroll";

            }
        }
        public byte[] NoImage
        {
            get
            {
                var res = HttpContext.Current.Server.MapPath("~/content/images/image-not-available.jpg");
                return File.ReadAllBytes(res);
            }
        }

    }
    public enum Genders
    {
        Male, Female
    }
    public enum StudentStatus
    {
        New, Transferee, Old
    }

}