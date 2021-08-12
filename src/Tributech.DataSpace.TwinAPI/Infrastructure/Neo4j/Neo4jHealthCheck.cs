using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j {
	public class Neo4jHealthCheck : IHealthCheck {
		private readonly IGraphClient _client;

		public Neo4jHealthCheck(IGraphClient client) {
			_client = client;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
			try {
				if (!_client.IsConnected) {
					await _client.ConnectAsync();
				}	
				await _client.Cypher.Return(() => Return.As<int>("1")).ResultsAsync;
				return HealthCheckResult.Healthy();
			}
			catch (Exception ex) {
				return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
			}
		}
	}
}
