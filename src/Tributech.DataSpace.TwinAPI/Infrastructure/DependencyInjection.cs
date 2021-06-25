using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;

namespace Tributech.DataSpace.TwinAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddOptions<VocabularyAPIOptions>().Bind(configuration.GetSection(nameof(VocabularyAPIOptions)));

            services
                .AddSingleton<ISchemaCache, InMemorySchemaCache>()
                .AddHttpContextAccessor()
                .AddTransient<VocabularyAPIAuthHandler>();

            services.AddHttpClient<IVocabularyService, VocabularyAPIClient>()
                .AddHttpMessageHandler<VocabularyAPIAuthHandler>();

            return services;
        }
    }
}
