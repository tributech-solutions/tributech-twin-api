using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;
using Neo4jClient;
using Microsoft.Extensions.Options;
using System;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DataSpace.TwinAPI.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Tributech.Dsk.CatalogApi.Client;

namespace Tributech.DataSpace.TwinAPI.Infrastructure {
	public static class DependencyInjection {
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<CatalogApiOptions>().Bind(configuration.GetSection(nameof(CatalogApiOptions)));
			services.AddOptions<Neo4jOptions>().Bind(configuration.GetSection(nameof(Neo4jOptions)));

			// singleton since recommended and thread safe according to docs
			// https://github.com/DotNet4Neo4j/Neo4jClient/wiki/connecting#threading-and-lifestyles
			services.AddSingleton<IGraphClient>(provider => {
				IOptions<Neo4jOptions> options = provider.GetService<IOptions<Neo4jOptions>>();
				ILogger<IGraphClient> logger = provider.GetRequiredService<ILogger<IGraphClient>>();
				IHostEnvironment env = provider.GetRequiredService<IHostEnvironment>();

				var client = new BoltGraphClient(new Uri(options.Value.Host), username: options.Value.User, password: options.Value.Password);

				// extended query logging during development
				if (env.IsDevelopment()) {
					client.OperationCompleted += (object sender, OperationCompletedEventArgs e) => {
						logger.LogDebug("Executed query {Query} in {QueryDuration}.", e.QueryText, e.TimeTaken);
					};
				}

				client.JsonConverters.Add(new DigitalTwinConverter());
				client.JsonConverters.Add(new RelationshipNodeConverter());

				return client;
			});

			services.AddScoped<ITwinRepository, TwinRepository>();
			services.AddScoped<IRelationshipRepository, RelationshipRepository>();
			services.AddScoped<IQueryRepository, QueryRepository>();

			services.AddHttpContextAccessor();
			services.AddTransient<CatalogApiAuthHandler>();
			services.AddHttpClient<CatalogApiClient>((sp, client) => {
					IOptions<CatalogApiOptions> options = sp.GetRequiredService<IOptions<CatalogApiOptions>>();
					client.BaseAddress = new Uri(options.Value.Url);
				})
				.AddHttpMessageHandler<CatalogApiAuthHandler>();

			return services;
		}
	}

	public class DigitalTwinConverter : JsonConverter<DigitalTwinNode> {
		public override DigitalTwinNode ReadJson(JsonReader reader, System.Type objectType, [AllowNull] DigitalTwinNode existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == "data") {
				reader.Read();
				var ojb = JToken.ReadFrom(reader);
				return ojb.ToObject<DigitalTwinNode>();
			}

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
	
	public class RelationshipNodeConverter : JsonConverter<RelationshipNode> {
		public override RelationshipNode ReadJson(JsonReader reader, System.Type objectType, [AllowNull] RelationshipNode existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == "data") {
				reader.Read();
				var ojb = JToken.ReadFrom(reader);
				return ojb.ToObject<RelationshipNode>();
			}

			var jo = JObject.Load(reader);
			var dataNode = jo["data"];
			var stringified = dataNode.ToString();
			return JsonConvert.DeserializeObject<RelationshipNode>(stringified);
		}

		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, [AllowNull] RelationshipNode value, JsonSerializer serializer) {
			throw new NotImplementedException();
		}
	}
}
