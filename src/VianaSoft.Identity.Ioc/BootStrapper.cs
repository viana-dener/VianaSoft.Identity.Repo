using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VianaSoft.BuildingBlocks.Core.Notifications.Interfaces;
using VianaSoft.BuildingBlocks.Core.Notifications;
using VianaSoft.BuildingBlocks.Core.User.Interfaces;
using VianaSoft.BuildingBlocks.Core.User;
using VianaSoft.Identity.App.AutoMapper;
using VianaSoft.Identity.App.Interfaces;
using VianaSoft.Identity.App.Services;
using VianaSoft.Identity.Data.Repositories;
using VianaSoft.Identity.Domain.Interfaces;
using VianaSoft.Identity.Domain.Services;
using VianaSoft.Identity.Services.SendGrid;
using AutoMapper;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.BuildingBlocks.Core.Resources;

namespace VianaSoft.Identity.Ioc
{
    public static class BootStrapper
    {
        public static IServiceCollection AddBootStrapper(this IServiceCollection services)
        {

            // Services
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILanguageMessage, LanguageMessage>();

            // Applications
            services.AddScoped<IIdentityApplication, IdentityApplication>();

            // Domain
            services.AddScoped<IIdentityService, IdentityService>();

            // Repository
            services.AddScoped<IIdentityRepository, IdentityRepository>();

            // Services
            services.AddScoped<ISendGridEmail, SendGridEmailService>();

            return services;
        }
    }
}
