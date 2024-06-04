using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Перехватывает и проверяет запросы на авторизованность пользователя
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var principal = jwtSecurityTokenHandler.ValidateToken(token, AuthHelper.GetValidationParameters(), out var validatedToken);

                if (principal == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
                context.Items["Principal"] = principal;
            }
            await _next(context);
        }        
    }
}
