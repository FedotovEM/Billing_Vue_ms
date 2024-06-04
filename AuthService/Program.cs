using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Helpers;
using AuthService.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("Cors", p => {
        p.WithOrigins("http://localhost:3000")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<BillingAuthDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BillingPostgreSQL")));

// Конфигурация
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = ConfigurationHelper.GetSectionValue("Jwt:Key");
var issuer = ConfigurationHelper.GetSectionValue("Jwt:Issuer");
var audience = ConfigurationHelper.GetSectionValue("Jwt:Audience");

// Добавление сервисов
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<CorsMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.Run();

