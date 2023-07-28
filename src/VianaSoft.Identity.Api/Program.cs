using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Serilog;
using Serilog.Events;
using VianaSoft.Identity.Api;

[assembly: ExcludeFromCodeCoverage]
var logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            //.WriteTo.AzureBlobStorage(connectionString: "DefaultEndpointsProtocol=https;AccountName=iwevaldevreports01;AccountKey=zu3GbRHIuqPdLXXV0mPZoWuRxQATOmGKuiP0Xzx9HN0hFCEFy5d0YXRLER9fQYpz9SR3jDljuxFO+ASt556Tww==;EndpointSuffix=core.windows.net",
            //                          storageContainerName: "VianaSoft-Dev-Logs",
            //                          storageFileName: "VianaSoft-Identity.log")
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
var localizationOptions = new RequestLocalizationOptions();
var supportedCultures = new[]
{
    new CultureInfo("pt-BR"),
    new CultureInfo("en-US"),
    new CultureInfo("es-ES"),
    new CultureInfo("fr-FR"),
};

localizationOptions.SupportedCultures = supportedCultures;
localizationOptions.SupportedUICultures = supportedCultures;
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
localizationOptions.SetDefaultCulture("pt-BR");

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.UseStartup<Startup>();