using Microsoft.OpenApi.Models;
using System.Reflection;

namespace VianaSoft.Identity.Api.Configuration
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var company = configuration.GetSection("ApplicationSettings:Application").Value;
            var apiName = configuration.GetSection("ApplicationSettings:ApiSettings:ApiName").Value;

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{company} {apiName}",
                    Description = $"API that maintains the registration of {apiName.ToLower()} in the {company}",
                    Contact = new OpenApiContact { Name = "VianaSoft Ltda.", Email = "viana.dener@gmail.com"}
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter the JWT token like this: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                s.EnableAnnotations();

                try
                {
                    var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xpath = Path.Combine(AppContext.BaseDirectory, xfile);
                    if (!string.IsNullOrWhiteSpace(xfile) && !string.IsNullOrWhiteSpace(xpath))
                        s.IncludeXmlComments(xpath);
                }
                catch
                {
                }
            });
        }
    }
}
