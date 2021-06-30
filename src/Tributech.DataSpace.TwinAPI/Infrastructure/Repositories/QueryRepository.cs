using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public class QueryRepository : IQueryRepository {
		private readonly ILogger<QueryRepository> _logger;
		private readonly Neo4jClient.IGraphClient _client;

		public QueryRepository(
			ILogger<QueryRepository> logger,
			Neo4jClient.IGraphClient graphClient) {
			_client = graphClient;
			_logger = logger;
		}

		public async Task<TwinGraph> GetSubgraph(TwinGraphQuery query) {
			var results = await _client.Cypher
			 .Match("(a:Twin {Id: $id})")
			 .WithParam("id", query.StartNodeDtId)
			 .Call("apoc.path.subgraphAll(a, {beginSequenceAtStart: true, relationshipFilter: $relationshipFilter, labelFilter: $labelFilter}) yield nodes, relationships")
			 .WithParams(new {
				 relationshipFilter = query.RelationshipFilter,
				 labelFilter = query.LabelFilter,
			 })
			 .Return((nodes, relationships) => new {
				 Nodes = nodes.As<DigitalTwinNode[]>(),
				 Relationships = relationships.As<RelationshipNode[]>()
			 })
			 .ResultsAsync;

			var mappedNodes = results.FirstOrDefault();
			var nodes = mappedNodes.Nodes.Select((DigitalTwinNode t) => t.MapToDigitalTwin());
			var rels = mappedNodes.Relationships.Select((RelationshipNode t) => t.MapToRelationship());

			return new TwinGraph() {
				Relationships = rels.ToArray(),
				DigitalTwins = nodes.ToArray()
			};
		}
	}
}
