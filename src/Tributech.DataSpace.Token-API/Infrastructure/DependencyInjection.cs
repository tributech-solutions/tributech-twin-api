using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;
using Neo4jClient;
using Microsoft.Extensions.Options;
using System;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;

namespace Tributech.DataSpace.TwinAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddOptions<VocabularyAPIOptions>().Bind(configuration.GetSection(nameof(VocabularyAPIOptions)));
			services.AddOptions<Neo4jOptions>().Bind(configuration.GetSection(nameof(Neo4jOptions)));

			services.AddScoped(typeof(IGraphClient), provider => {
				var options = provider.GetService<IOptions<Neo4jOptions>>();
				var client = new BoltGraphClient(new Uri(options.Value.Host), username: options.Value.User, password: options.Value.Password);
				client.ConnectAsync().Wait();

				return client;
			});


			services.AddScoped<ITwinRepository, TwinRepository>();

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
