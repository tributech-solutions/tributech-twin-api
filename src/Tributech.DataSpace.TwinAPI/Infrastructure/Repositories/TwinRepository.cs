using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public class TwinRepository : ITwinRepository {
		private readonly ILogger<TwinRepository> _logger;
		private readonly IGraphClient _client;

		public TwinRepository(
			ILogger<TwinRepository> logger,
			IGraphClient graphClient) {
			_client = graphClient;
			_logger = logger;
		}

		public async Task<DigitalTwin> CreateTwinAsync(DigitalTwin twin) {
			var results = await _client.Cypher
				.Merge("(twin:Twin {Id: $id})")
				.Set("twin = $twin")
				.WithParam("twin", twin.ToDotNotationDictionary())
				.WithParam("id", twin.Id)
				.Return((twin) => twin.As<DigitalTwinNode>())
				.ResultsAsync;

			var mappedResults = results.MapToDigitalTwin();
			return mappedResults.FirstOrDefault();
		}

		public async Task<DigitalTwin> DeleteTwinAsync(Guid twinId) {
			var results = await _client.Cypher
			 .Match("(twin:Twin)")
			 .Where((DigitalTwinNode twin) => twin.Id == twinId)
			 .DetachDelete("twin")
			 .Return((twin) => twin.As<DigitalTwinNode>())
			 .ResultsAsync;

			var mappedResults = results.MapToDigitalTwin();
			return mappedResults.FirstOrDefault();
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

		public async Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginatedAsync(uint pageNumber, uint pageSize) {
			var results = await _client.Cypher
				.Match("(twin:Twin)")
				.Return((twin) => twin.As<DigitalTwinNode>())
				.ResultsAsync;
			var mappedResults = results.MapToDigitalTwin();
			return new PaginatedResponse<DigitalTwin>(mappedResults.Count(), mappedResults);
		}

		public async Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin) {
			var results = await _client.Cypher
				.Merge("(twin:Twin {Id: $id})")
				.OnMatch()
				.Set("twin = $twin")
				.WithParam("twin", twin.ToDotNotationDictionary())
				.WithParam("id", twin.Id)
				.Return((twin) => twin.As<DigitalTwinNode>())
				.ResultsAsync;
			var mappedResults = results.MapToDigitalTwin();
			return mappedResults.FirstOrDefault();
		}

		public async Task<PaginatedResponse<DigitalTwin>> GetTwinsByModelPaginatedAsync(string dtmi, uint pageNumber, uint pageSize) {
			var results = await _client.Cypher
				.Match("(twin:Twin {ModelId: $id})")
				.WithParam("id", dtmi)
				.Return((twin) => twin.As<DigitalTwinNode>())
				.ResultsAsync;
			var mappedResults = results.MapToDigitalTwin();
			return new PaginatedResponse<DigitalTwin>(mappedResults.Count(), mappedResults);
		}

	}
}
