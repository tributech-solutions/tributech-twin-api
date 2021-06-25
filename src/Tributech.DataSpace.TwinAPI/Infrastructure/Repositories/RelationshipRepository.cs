using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public class RelationshipRepository : IRelationshipRepository {
		private readonly ILogger<TwinRepository> _logger;
		private readonly Neo4jClient.IGraphClient _client;

		public RelationshipRepository(
			ILogger<TwinRepository> logger,
			Neo4jClient.IGraphClient graphClient) {
			_client = graphClient;
			_logger = logger;
		}

		public async Task<Relationship> CreateRelationshipAsync(Relationship relationship) {
			var results = await _client.Cypher
				 .Match("(sourceTwin:Twin)", "(targetTwin:Twin)")
				 .Where((DigitalTwinNode sourceTwin) => sourceTwin.Id == relationship.SourceId)
				 .AndWhere((DigitalTwinNode targetTwin) => targetTwin.Id == relationship.TargetId)
				 .Merge($"(sourceTwin)-[r:{relationship.Name} {{ Id: $id }}]->(targetTwin)")
					.Set("r = $rel")
					.WithParam("rel", relationship.GetFlat())
					.WithParam("id", relationship.Id)
				 .Return((r) => r.As<Relationship>())
				 .ResultsAsync;

			//var mappedResults = MapToDigitalTwin(results);
			return results.FirstOrDefault();
		}

		public async Task<Relationship> DeleteRelationshipAsync(Guid relationshipId) {
			var results = await _client.Cypher
			 .Match("(:Twin)--[rel {Id: $id}]--(:Twin)")
			 .WithParam("id", relationshipId)
			 .Delete("rel")
			 .Return((rel) => rel.As<Relationship>())
			 .ResultsAsync;

			return results.FirstOrDefault();
		}

		public async Task<Relationship> GetRelationshipAsync(Guid relationshipId) {
			var results = await _client.Cypher
		 .Match("(:Twin)-[r {{ Id: $id }}]-(:Twin)")
		 .WithParam("id", relationshipId)
		 .Return((r) => r.As<Relationship>())
		 .ResultsAsync;

			//var mappedResults = MapToDigitalTwin(results);
			return results.FirstOrDefault();
		}

		public async Task<Relationship> UpsertRelationshipAsync(Relationship relationship) {
			var results = await _client.Cypher
		 .Match("(sourceTwin:Twin)", "(targetTwin:Twin)")
		 .Where((DigitalTwinNode sourceTwin) => sourceTwin.Id == relationship.SourceId)
		 .AndWhere((DigitalTwinNode targetTwin) => targetTwin.Id == relationship.TargetId)
		 .Merge($"(sourceTwin)-[r:{relationship.Name} {{ Id: $id }}]->(targetTwin)")
			.Set("r = $rel")
			.WithParam("rel", relationship.GetFlat())
			.WithParam("id", relationship.Id)
		 .Return((r) => r.As<Relationship>())
		 .ResultsAsync;

			//var mappedResults = MapToDigitalTwin(results);
			return results.FirstOrDefault();
		}
	}
}
