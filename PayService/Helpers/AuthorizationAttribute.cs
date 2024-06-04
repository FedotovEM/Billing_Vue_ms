using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace PayService.Helpers
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
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }            
        }
    }
}
