using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AuthService.Helpers
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Проверяет тело запроса на авторизованность пользователя, отправившего его 
        /// </summary>
        /// <param name="context">Контекст авторизации запроса</param>
        /// <returns></returns>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items.ContainsKey("Principal"))
            {
                var principal = context.HttpContext.Items["Principal"] as ClaimsPrincipal;
                if (principal == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
