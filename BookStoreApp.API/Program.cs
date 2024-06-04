using BookStoreApp.API;
using BookStoreApp.API.Configuration;
using BookStoreApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Obtenir chaine de connexion
var connectionString = builder.Configuration.GetConnectionString("BookStoreDb");
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(MapperConfig));

// Sécurité

builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // quiconque veut accéder à une ressource protégée doit être authentifié avec un Bearer
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // quiconque veut challenger un utilisateur doit être authentifié avec un Bearer
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,  // on veut vérifier que la clé utilisée pour signer le token est une clé valide
        ValidateIssuer = true,  // on veut vérifier que l'émetteur du token est valide
        ValidateAudience = true, // on veut vérifier que le destinataire du token est valide
        ValidateLifetime = true, // on veut vérifier que le token n'est pas expiré
        ClockSkew = TimeSpan.Zero, // on tolère un décalage de 0 minute
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"], // on vérifie que l'émetteur du token est bien celui qu'on attend
        ValidAudience = builder.Configuration["JwtSettings:Audience"], // on vérifie que le destinataire du token est bien celui qu'on attend
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)) // on vérifie que la clé utilisée pour signer le token est bien celle qu'on attend
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
