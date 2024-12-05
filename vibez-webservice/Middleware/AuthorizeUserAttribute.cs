using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SOA_CA2.Middleware
{
    /// <summary>
    /// Authorization filter to ensure the user is authenticated.
    /// </summary>
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ClaimsPrincipal? claimsPrincipal = context.HttpContext.Items["User"] as ClaimsPrincipal;

            if (claimsPrincipal == null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
