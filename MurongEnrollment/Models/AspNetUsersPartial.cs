using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MurongEnrollment.Models
{
    public partial class AspNetUsers
    {
        public string FullName { get { return this.AspNetUserDetails?.FirstName + " " + this.AspNetUserDetails?.LastName; } }
        public string Roles
        {
            get
            {
                var roles = "";
                roles = string.Join(",", this.AspNetRoles.Select(x => x.Name));
                return roles;
            }
        }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string UserRoles { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string MiddleName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
    }
}