using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace NachislService.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _authProjectUrl = Environment.GetEnvironmentVariable("AuthService_URL");

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
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_authProjectUrl);
                var response = await client.GetAsync($"/api/authorization/check-token?token={token}");
                response.EnsureSuccessStatusCode();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token");
                }

                await _next(context);
            }
        }

    }
}
