using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("Pretest")));

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IJwtService, JwtService>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPendingAccountRepository, PendingAccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IFarmRepository, FarmRepository>();
            services.AddScoped<IHorticultureClassRepository, HorticultureClassRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUomRepository, UomRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPondRepository, PondRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IQcCategoryRepository, QcCategoryRepository>();
            services.AddScoped<IQcFormRepository, QcFormRepository>();
            services.AddScoped<IQcSectionRepository, QcSectionRepository>();
            services.AddScoped<IQcQuestionRepository, QcQuestionRepository>();
            services.AddScoped<IQcAnswerRepository, QcAnswerRepository>();
            services.AddScoped<IQcResponseRepository, QcResponseRepository>();
            services.AddScoped<IQcAnswerTypeRepository, QcAnswerTypeRepository>();
            services.AddScoped<IQcTypeRepository, QcTypeRepository>();
            services.AddScoped<IDispatchRepository, DispatchRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var jwtSettings = config.GetSection("JwtSettings");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },

                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";

                        var customResult = new
                        {
                            isSuccess = false,
                            isFailure = true,
                            statusCode = (int)HttpStatusCode.Unauthorized,
                            message = "Authentication failed. The provided token is invalid or missing.",
                            value = (object?)null
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(customResult));
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";

                        var customResult = new
                        {
                            isSuccess = false,
                            isFailure = true,
                            statusCode = (int)HttpStatusCode.Forbidden,
                            message = "Access denied. You do not have permission to access this resource.",
                            value = (object?)null
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(customResult));
                    }
                };
            });

            return services;
        }
    }
}
