using Application.Common.Constants;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        private const string CONNECTION_STRING_NAME = "DefaultConnection";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString(CONNECTION_STRING_NAME),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>();
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthorizationConstants.JWT_SECRET_KEY))
                            };
                        });
                services.AddAuthorization();

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<ApplicationDbContextInitialiser>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ITokenClaimService, IdentityTokenClaimService>();

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
