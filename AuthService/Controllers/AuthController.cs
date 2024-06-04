using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using AuthService.Helpers;
using AuthService.models;
using AuthService.Repository;
using AuthService.Repository.Models;
using System.Net;

namespace AuthService.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly BillingAuthDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(BillingAuthDbContext context, IConfiguration configuration)
        {
            _context = context;           
            _configuration = configuration;
        }

        /// <summary>
        /// Производит вход в систему по логину и паролю
        /// </summary>
        /// <param name="model">Модель входа в систему, содержащая поля логина и пароля</param>
        /// <returns>Возвращает объект с токеном авторизации и параметрами, характеризующими пользователя</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);

            if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, "Пользователь с такими учётными данными не найден");

            var token = AuthHelper.generateJWTToken(user.UserName, user.UserRole);
            var strToken = new JwtSecurityTokenHandler().WriteToken(token);

            var responseUser = new AuthResponse(user, strToken);

            return Ok(new
            {
                token = strToken,
                expiration = token.ValidTo,
                username = responseUser.UserName,
                id = responseUser.UserID,
                email = responseUser.Email,
                roles = responseUser.UserRole
            });
        }

        /// <summary>
        /// Производит регистрацию в системе по логину, паролю и email
        /// </summary>
        /// <param name="model">Модель регистрации в системе, содержащая поля логина, пароля и email</param>
        /// <returns>Возвращает код статуса регистрации пользователя</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            var userExists = _context.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Пользователь с таким логином уже существует!");

            var newUser = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Password = model.Password,
                UserRole = "User"
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("Пользователь создан успешно");
        }

        /// <summary>
        /// Производит проверку токена авторизации
        /// </summary>
        /// <param name="token">Токен авторизации</param>
        /// <returns>Возвращает HTTP код статуса</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("check-token")]
        public HttpStatusCode CheckToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return HttpStatusCode.Unauthorized;

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var principal = jwtSecurityTokenHandler.ValidateToken(token, AuthHelper.GetValidationParameters(), out var validatedToken);

            if (principal == null)
                return HttpStatusCode.Unauthorized;

            return HttpStatusCode.OK;
        }
    }
}
