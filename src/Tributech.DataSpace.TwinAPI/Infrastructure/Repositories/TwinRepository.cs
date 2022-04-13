using Neo4jClient;
using Neo4jClient.Cypher;
using Tributech.DataSpace.TwinAPI.Application.Schema;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public class TwinRepository : ITwinRepository {
		private readonly ILogger<TwinRepository> _logger;
		private readonly IGraphClient _client;
		private readonly ISchemaService _schemaService;

		public TwinRepository(
			ILogger<TwinRepository> logger,
			IGraphClient graphClient,
			ISchemaService schemaService
			) {
			_client = graphClient;
			_schemaService = schemaService;
			_logger = logger;
		}

		public async Task DeleteTwinAsync(Guid twinId) {
			await _client.Cypher
			 .Match("(twin:Twin)")
			 .Where((DigitalTwinNode twin) => twin.Id == twinId)
			 .DetachDelete("twin")
			 .ExecuteWithoutResultsAsync();
		}

		public async Task<DigitalTwin> GetTwinAsync(Guid twinId) {
			var results = await _client.Cypher
				.Match("(twin:Twin)")
				.Where((DigitalTwinNode twin) => twin.Id == twinId)
				.Return((twin) => twin.As<DigitalTwinNode>())
				.ResultsAsync;

			var mappedResults = results.MapToDigitalTwin();
			return mappedResults.FirstOrDefault();
		}

		public async Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginatedAsync(int pageNumber, int pageSize) {
			ICypherFluentQuery baseQuery = _client.Cypher
							.Match("(twin:Twin)");

			long count = (await baseQuery
					.Return(twin => twin.Count())
					.ResultsAsync)
				.First();

			IEnumerable<DigitalTwinNode> results = await baseQuery
							.Return((twin) => twin.As<DigitalTwinNode>())
							.OrderBy("twin.ModelId", "twin.Id")
							.Skip((pageNumber - 1) * pageSize)
							.Limit(pageSize)
							.ResultsAsync;

			IEnumerable<DigitalTwin> mappedResults = results.MapToDigitalTwin();
			return new PaginatedResponse<DigitalTwin>(count, mappedResults);
		}

		public async Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken) {
			
			List<string> labels = new List<string>();

			// add twin model type (DTMI) as label
			labels.Add(twin.Metadata.ModelId.ToLabel());

			// add twin base model types (DTMI) as labels
			IEnumerable<string> baseModelTypes = await _schemaService.GetBaseModels(twin.Metadata.ModelId, cancellationToken);
			labels.AddRange(baseModelTypes.Select(dtmi => dtmi.ToLabel()).ToArray());

			var results = await _client.Cypher
					.Merge("(twin:Twin {Id: $id})")
					.Set("twin = $twin")
					.Set($"twin:{string.Join(":", labels)}") // labels do not fully support upsert (no removal of existing labels)
					.WithParam("twin", twin.ToDotNotationDictionary())
					.WithParam("id", twin.Id)
					.Return((twin) => twin.As<DigitalTwinNode>())
					.ResultsAsync;
			var mappedResults = results.MapToDigitalTwin();
			return mappedResults.FirstOrDefault();
		}

		public async Task<PaginatedResponse<DigitalTwin>> GetTwinsByModelPaginatedAsync(string dtmi, int pageNumber, int pageSize) {
			ICypherFluentQuery baseQuery = _client.Cypher
							.Match("(twin:Twin {ModelId: $id})")
							.WithParam("id", dtmi);

			long count = (await baseQuery
					.Return(twin => twin.Count())
					.ResultsAsync)
				.First();

			IEnumerable<DigitalTwinNode> results = await baseQuery
							.Return((twin) => twin.As<DigitalTwinNode>())
							.OrderBy("twin.Id")
							.Skip((pageNumber - 1) * pageSize)
							.Limit(pageSize)
							.ResultsAsync;

			IEnumerable<DigitalTwin> mappedResults = results.MapToDigitalTwin();
			return new PaginatedResponse<DigitalTwin>(count, mappedResults);
		}

	}
}
