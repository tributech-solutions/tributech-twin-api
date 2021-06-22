using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public class TwinRepository : ITwinRepository {
		private readonly ILogger<Neo4jRepository<DigitalTwin>> _logger;
		private readonly IGraphClient _client;

		public TwinRepository(
			ILogger<Neo4jRepository<DigitalTwin>> logger,
			IGraphClient graphClient) {
			_client = graphClient;
			_logger = logger;
		}

		public Task<RelationshipDto> CreateRelationshipAsync() {
			throw new NotImplementedException();
		}

		public async Task<DigitalTwin> CreateTwinAsync(DigitalTwin twin) {
			return (await _client.Cypher
				.Merge("(twin:Twin {Id: $id})")
				.OnCreate()
				.Set("twin = $twin")
				.WithParam("twin", twin.GetFlat())
				.WithParam("id", twin.Id)
				.Return((twin) => twin.As<DigitalTwin>()).ResultsAsync).FirstOrDefault();
		}

		public Task<RelationshipDto> DeleteRelationshipAsync() {
			throw new NotImplementedException();
		}

		public Task<DigitalTwin> DeleteTwinAsync(Guid twinId) {
			throw new NotImplementedException();
		}

		public Task<DigitalTwin> GetTwin(Guid twinId) {
			throw new NotImplementedException();
		}

		public async Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginated(uint pageNumber, uint pageSize) {
			var results =  await _client.Cypher
				.Match("(twin:Twin)")
				.Return((twin) => twin.As<DigitalTwin>())
				.Skip((int)pageNumber * (int)pageSize)
				.Limit((int)pageSize)
				.ResultsAsync;

			return new PaginatedResponse<DigitalTwin>(results.Count(), results);
		}

		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin) {
			throw new NotImplementedException();
		}

		private object ConvertDictionaryTo(IDictionary<string, string> dictionary) {
			dynamic eo = dictionary.Aggregate(new ExpandoObject() as IDictionary<string, object>,
									(a, p) => { a.Add(p.Key, p.Value); return a; });
			return eo;
		}
	}
}
