using System.Text;
using FluentValidation;
using LibraryManagement.API.Middlewares;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Validators;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Services;
using LibraryManagement.Infrastructure.Settings;
using LibraryManagement.Persistence;
using LibraryManagement.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LibraryManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load .env file in development
            if (builder.Environment.IsDevelopment())
            {
                DotNetEnv.Env.Load();
            }

            // Add environment variables to configuration
            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management API", Version = "v1" });

                // Add JWT Authentication support to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "ouath2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBorrowingRequestService, BorrowingRequestService>();
            builder.Services.AddScoped<IRequestDetailsService, RequestDetailsService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IValidator<BookToUpdateDTO>, BookToUpdateDTOValidator>();
            builder.Services.AddScoped<IValidator<BookToAddDTO>, BookToAddDTOValidator>();
            builder.Services.AddScoped<IValidator<CategoryToUpdateDTO>, CategoryToUpdateDTOValidator>();
            builder.Services.AddScoped<IValidator<CategoryToAddDTO>, CategoryToAddDTOValidator>();
            builder.Services.AddScoped<IValidator<UserToRegisterDTO>,  UserToRegisterDTOValidator>();
            builder.Services.AddScoped<IValidator<UserToLoginDTO>, UserToLoginDTOValidator>();

            builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryManagementDBConnection"));
            });

            // Configure JWT settings
            builder.Services.Configure<JwtSettings>(options =>
            {
                options.Secret = Environment.GetEnvironmentVariable("JWT_SECRET")
                   ?? builder.Configuration["JwtSettings:Secret"]
                   ?? throw new InvalidOperationException("JWT Secret is not configured");

                options.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                   ?? builder.Configuration["JwtSettings:Issuer"]
                   ?? throw new InvalidOperationException("JWT Issuer is not configured");

                options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                   ?? builder.Configuration["JwtSettings:Audience"]
                   ?? throw new InvalidOperationException("JWT Audience is not configured");

                options.ExpiryMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES"), out var expiry) 
                    ? expiry 
                    : builder.Configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 120);
            });

            // Add JWT Authentication     
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
            }).AddJwtBearer(options =>
            {
                var jwtSettings = new JwtSettings
                {
                    Secret = Environment.GetEnvironmentVariable("JWT_SECRET")
                        ?? builder.Configuration["JwtSettings:Secret"]
                        ?? default!,
                    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                        ?? builder.Configuration["JwtSettings:Issuer"]
                        ?? default!,
                    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                        ?? builder.Configuration["JwtSettings:Audience"]
                        ?? default!,
                    ExpiryMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES"), out var expiry)
                        ? expiry
                        : builder.Configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 120)
                };
                
                var key = Encoding.ASCII.GetBytes(jwtSettings?.Secret ?? default!);
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings?.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            var libraryManagementClientOrigins = "_libraryManagementClientOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(libraryManagementClientOrigins, policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management API v1");
                });
            }

            app.UseCors(libraryManagementClientOrigins);

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>(); 

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
