using FluentValidation.AspNetCore;

namespace VianaSoft.Identity.Api.Configuration
{
    [Obsolete]
    public static class FluentValidationSetup
    {
        public static IServiceCollection AddFluentValidationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddFluentValidation(options =>
            {
                // Validate child properties and root collection elements
                options.ImplicitlyValidateChildProperties = true;
                options.ImplicitlyValidateRootCollectionElements = true;

                // Automatic registration of validators in assembly
                options.RegisterValidatorsFromAssemblyContaining<Startup>();

            });

            return services;
        }
    }
}
