using Neo4jClient.Cypher;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

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

		public async Task DeleteRelationshipAsync(Guid relationshipId) {
			await _client.Cypher
			 .Match("(:Twin)-[rel {Id: $id}]-(:Twin)")
			 .WithParam("id", relationshipId)
			 .Delete("rel")
			 .ExecuteWithoutResultsAsync();
		}

		public async Task<Relationship> GetRelationshipAsync(Guid relationshipId) {
			var results = await _client.Cypher
			 .Match("(:Twin)-[r { Id: $id }]-(:Twin)")
			 .WithParam("id", relationshipId)
			 .Return((r) => r.As<RelationshipNode>())
			 .ResultsAsync;

			var mappedResults = results.MapToRelationship();
			return mappedResults.FirstOrDefault();
		}
		
		public async Task<IEnumerable<Relationship>> GetOutgoingRelationshipsAsync(Guid twinId) {
			var results = await _client.Cypher
				.Match("(:Twin)-[r { SourceId: $id }]-(:Twin)")
				.WithParam("id", twinId)
				.Return((r) => r.As<RelationshipNode>())
				.ResultsAsync;

			var mappedResults = results.MapToRelationship();
			return mappedResults;
		}
		
		public async Task<IEnumerable<Relationship>> GetIncomingRelationshipsAsync(Guid twinId) {
			var results = await _client.Cypher
				.Match("(:Twin)-[r { TargetId: $id }]-(:Twin)")
				.WithParam("id", twinId)
				.Return((r) => r.As<RelationshipNode>())
				.ResultsAsync;

			var mappedResults = results.MapToRelationship();
			return mappedResults;
		}

		public Task<Relationship> CreateRelationshipAsync(Relationship relationship)
			=> UpsertRelationshipAsync(relationship);

		public async Task<Relationship> UpsertRelationshipAsync(Relationship relationship) {
			var results = await _client.Cypher
			 .Match("(sourceTwin:Twin)", "(targetTwin:Twin)")
			 .Where((DigitalTwinNode sourceTwin) => sourceTwin.Id == relationship.SourceId)
			 .AndWhere((DigitalTwinNode targetTwin) => targetTwin.Id == relationship.TargetId)
			 .Merge($"(sourceTwin)-[r:{relationship.Name} {{ Id: $id }}]->(targetTwin)")
				.Set("r = $rel")
				.WithParam("rel", relationship.ToDotNotationDictionary())
				.WithParam("id", relationship.Id)
			 .Return((r) => r.As<RelationshipNode>())
			 .ResultsAsync;

			var mappedResults = results.MapToRelationship();
			return mappedResults.FirstOrDefault();
		}

		public async Task<PaginatedResponse<Relationship>> GetRelationshipsPaginatedAsync(int pageNumber, int pageSize) {
			ICypherFluentQuery baseQuery = _client.Cypher
							.Match("(:Twin)-[relationship]-(:Twin)");

			long count = (await baseQuery
					.Return(relationship => relationship.CountDistinct())
					.ResultsAsync)
				.First();

			IEnumerable<RelationshipNode> results = await baseQuery
							.ReturnDistinct((relationship) => relationship.As<RelationshipNode>())
							.OrderBy("relationship.Name", "relationship.Id", "relationship.SourceId", "relationship.TargetId")
							.Skip((pageNumber - 1) * pageSize)
							.Limit(pageSize)
							.ResultsAsync;

			IEnumerable<Relationship> mappedResults = results.MapToRelationship();
			return new PaginatedResponse<Relationship>(count, mappedResults);
		}
	}
}
