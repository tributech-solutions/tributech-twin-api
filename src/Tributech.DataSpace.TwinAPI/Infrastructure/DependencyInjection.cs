using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;
using Neo4jClient;
using Microsoft.Extensions.Options;
using System;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Newtonsoft.Json;
using Tributech.DataSpace.TwinAPI.Application.Model;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Relationship = Tributech.DataSpace.TwinAPI.Application.Model.Relationship;

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


				client.JsonConverters.Add(new DigitalTwinConverter());
				client.JsonConverters.Add(new RelationshipConverter());

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

	public class DigitalTwinConverter : JsonConverter<DigitalTwinNode> {
		public override DigitalTwinNode ReadJson(JsonReader reader, Type objectType, [AllowNull] DigitalTwinNode existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var jo = JObject.Load(reader);
			var dataNode = jo["data"];
			var stringified = dataNode.ToString();
			return JsonConvert.DeserializeObject<DigitalTwinNode>(stringified);
		}

		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, [AllowNull] DigitalTwinNode value, JsonSerializer serializer) {
			throw new NotImplementedException();
		}
	}

	public class RelationshipConverter : JsonConverter<Relationship> {
		public override Relationship ReadJson(JsonReader reader, Type objectType, [AllowNull] Relationship existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var jo = JObject.Load(reader);
			var dataNode = jo["data"];
			var stringified = dataNode.ToString();
			return JsonConvert.DeserializeObject<Relationship>(stringified);
		}

		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, [AllowNull] Relationship value, JsonSerializer serializer) {
			throw new NotImplementedException();
		}
	}
}
