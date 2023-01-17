using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStore.Application.AppCode.Extenstions
{
    public static partial class Extension
    {
        public static string GetPrincipalName(this ClaimsPrincipal principal)
        {
            string name = principal.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            string surname = principal.Claims.FirstOrDefault(c => c.Type.Equals("surname"))?.Value;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
            {
                return $"{name} {surname}";
            }

            return principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        }

        public static bool HasAccess(this ClaimsPrincipal principal, string policyName)
        {
            if (principal.IsInRole("sa"))
            {
                return true;
            }
            return principal.Claims.Any(c => c.Type.Equals(policyName) && c.Value.Equals("1"));
        }

        public static int GetCurrentUserId(this ClaimsIdentity identity)
        {
            return Convert.ToInt32(
                    identity.Claims.FirstOrDefault(c =>
                    c.Type.Equals(ClaimTypes.NameIdentifier)).Value
                    );
        }


        public static int GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity is ClaimsIdentity identity)
            {
                return identity.GetCurrentUserId();
            }

            return 0;
        }


        public static int GetCurrentUserId(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.HttpContext.User.GetCurrentUserId();
        }
    }
}
