using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Helpers
{
    public class AuthHelper
    {
        /// <summary>
        /// Генерирует токен авторизации
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="Role">Роль пользователя</param>
        /// <returns>Объект JWT-токена авторизации</returns>
        public static SecurityToken generateJWTToken(string Login, string Role)
        {
            var secretKey = ConfigurationHelper.GetSectionValue("Jwt:Key");
            var issuer = ConfigurationHelper.GetSectionValue("Jwt:Issuer");
            var audience = ConfigurationHelper.GetSectionValue("Jwt:Audience");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Login),
                new Claim(ClaimTypes.Role, Role),
                new Claim("custom:claim", "custom value"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        /// <summary>
        /// Получает параметры проверки токена
        /// </summary>
        /// <returns>Объект параметров проверки JWT-токена авторизации</returns>
        public static TokenValidationParameters GetValidationParameters()
        {
            var secretKey = ConfigurationHelper.GetSectionValue("Jwt:Key");
            var issuer = ConfigurationHelper.GetSectionValue("Jwt:Issuer");
            var audience = ConfigurationHelper.GetSectionValue("Jwt:Audience");
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = symmetricSecurityKey,
            };
        }
    }
}
