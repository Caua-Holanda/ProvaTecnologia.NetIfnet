using Microsoft.Extensions.Options;
using ProvaMed.DomainModel.Interfaces.UoW;
using ProvaMed.Infra.UoW;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;
using ProvaMedGroup.DomainModel.Interfaces.Services;
using ProvaMedGroup.DomainService;
using ProvaMedGroup.Infra.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace ProvaMed.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ProvaMedGroupDbContext>();

            // Unit Of Work
            services.AddScoped<IUnitOfWork, EntityFrameworkUnitOfWork>();


            //Swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

         
            // Contato
            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IContatoService, ContatoService>();

            
            return services;
        }
    }
}

