using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Op.Repository;
using Op.Repository.IRepostiory;
using OP.DTO.Mapper;
using OP.Services;
using OP.Services.CredentialService;
using OP.Services.Interfaces;
using OP.Services.OperatorServices;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson();

// example for extraction
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//string hrServerPort = configuration["HR_SERVER_PORT"];
var secretKey = configuration["token:SecretKey"];

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\" ",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configure DbContext
builder.Services.AddDbContext<DeviseHrContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DeviseDB")));

// configure auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Check if the token is expired
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Sudo", policy =>
    policy.RequireClaim("userRole", "50"));

    options.AddPolicy("Admin", policy =>
    policy.RequireClaim("userRole", "50", "40"));

    options.AddPolicy("Manager", policy =>
    policy.RequireClaim("userRole", "50", "40", "30"));

    options.AddPolicy("Employee", policy =>
    policy.RequireClaim("userRole", "50", "40", "30", "20"));

    options.AddPolicy("Visitor", policy =>
    policy.RequireClaim("userRole", "50", "40", "30", "20", "10"));
});

// add the CORS middleware to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();

    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});


// Scope and Dependancy Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

// Register mappings
MapConfig.RegisterMappings();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// must be above authorization
app.UseCors("AllowAnyOrigin");

// must be above authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();






app.Run();