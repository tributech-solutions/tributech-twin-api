using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Polly;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j {
	public static class Neo4jBootstrapExtensions {

		/// <summary>
		/// Helper for boostraping (e.g. creating constraints) of Neo4j graph database.
		/// </summary>
		/// <param name="host"></param>
		/// <returns></returns>
		public static async Task<IHost> BootsrapNeo4j(this IHost host) {
			using IServiceScope serviceScope = host.Services.CreateScope();
			ILogger logger = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Neo4jBoostrap");
			logger.LogDebug("Starting Neo4j bootstrapping...");
			try {
				IGraphClient neo4jClient = serviceScope.ServiceProvider.GetRequiredService<IGraphClient>();
				if (!neo4jClient.IsConnected) {
					logger.LogDebug("Trying to connect to Neo4j database...");

					await Policy
						.Handle<Exception>()
						.WaitAndRetryAsync(5, 
							retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
							onRetry: (ex,t) => logger.LogWarning(ex, "Error during establishing connection to Neo4j database. Retry in {WaitDuration}s...", t.TotalSeconds)
						)
						.ExecuteAsync(() => neo4jClient.ConnectAsync());

					logger.LogDebug("Connected to Neo4j database.");
				}
				await CreateConstraints(neo4jClient);
			}
			catch (Exception ex) {
				logger.LogError(ex, "Error during Neo4j boostrapping.");
				Environment.Exit(1);
			}
			logger.LogInformation("Finished Neo4j bootstrapping.");
			return host;
		}

		private static async Task CreateConstraints(IGraphClient neo4jClient) {
			await neo4jClient.Cypher.Create("CONSTRAINT UQ_Twin IF NOT EXISTS ON (twin:Twin) ASSERT twin.Id IS UNIQUE;").ExecuteWithoutResultsAsync();
		}

	}
}

