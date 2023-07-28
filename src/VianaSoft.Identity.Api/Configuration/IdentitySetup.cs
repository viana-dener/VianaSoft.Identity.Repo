using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VianaSoft.BuildingBlocks.Core.Configuration;
using VianaSoft.Identity.Data.Context;
using VianaSoft.Identity.Domain.Entities;

namespace VianaSoft.Identity.Api.Configuration
{
    public static class IdentitySetup
    {
        public static IServiceCollection AddIdentitySetup(this IServiceCollection services, IConfiguration configuration)
        {
            var apiSettingsSection = configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(apiSettingsSection);

            services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                var time = configuration.GetSection("ApplicationSettings:ApiSettings:ExpirationHours").Value;

                options.TokenLifespan = TimeSpan.FromHours(int.Parse(time)); // Defina a vida útil do token conforme necessário
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IdentityDataContext>();
            services.AddJwt(configuration);

            return services;
        }
    }
}
