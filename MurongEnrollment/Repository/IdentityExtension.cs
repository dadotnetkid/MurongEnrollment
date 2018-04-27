using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MurongEnrollment
{
    public static class IdentityExtension
    {
        public static string GetFullName(this IIdentity user)
        {
            var claim = user as ClaimsIdentity;
            return claim.FindFirst("FullName").Value;
        }
        public static string GetRoles(this IIdentity user)
        {
            var claim = user as ClaimsIdentity;
            return claim.FindFirst("Roles").Value;
        }
        public static bool IsInRoles(this IIdentity user, params string[] roles)
        {
            foreach (var i in user.GetRoles().Split(','))
            {
                if (roles.Contains(i))
                {
                    return true;
                }
            }
            return false;
        }
    }
}