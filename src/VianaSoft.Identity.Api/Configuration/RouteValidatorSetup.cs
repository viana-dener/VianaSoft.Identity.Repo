using FluentValidation;
using VianaSoft.Identity.Api.Validations;
using VianaSoft.Identity.App.Models.Request;

namespace VianaSoft.Identity.Api.Configuration
{
    public static class RouteValidatorSetup
    {
        public static IServiceCollection AddRouteValidator(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IValidator<UserLoginRequest>, UserLoginRouteValidator>();
            services.AddTransient<IValidator<UserRegistrationRequest>, UserRegistrationRouteValidator>();
            services.AddTransient<IValidator<ForgotPasswordRequest>, ForgotPasswordRouteValidator>();
            services.AddTransient<IValidator<ResetPasswordRequest>, ResetPasswordRouteValidator>();
            services.AddTransient<IValidator<ChangePasswordRequest>, ChangePasswordRouteValidator>();
            services.AddTransient<IValidator<ResetPasswordTokenValidRequest>, ResetPasswordTokenValidRouteValidator>();

            return services;
        }
    }
}
