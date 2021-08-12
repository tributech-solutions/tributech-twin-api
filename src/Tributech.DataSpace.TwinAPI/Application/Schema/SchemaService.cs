using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tributech.Dsk.CatalogApi.Client;

namespace Tributech.DataSpace.TwinAPI.Application.Schema {
	public class SchemaService : ISchemaService {
		private readonly CatalogApiClient _catalogAPIClient;
		// schemas are immutable that's why we cache them forever and use a dict
		// mapping from model type to base model types (DTMI)
		private readonly ConcurrentDictionary<string, IEnumerable<string>> _baseModels = new ConcurrentDictionary<string, IEnumerable<string>>();

		public SchemaService(CatalogApiClient catalogAPIClient) {
			_catalogAPIClient = catalogAPIClient;
		}

		public async Task<IEnumerable<string>> GetBaseModels(string dtmi, CancellationToken cancellationToken = default) {
			if (!_baseModels.TryGetValue(dtmi, out IEnumerable<string> baseModels)) {
				// in a worst case retrieval could happen multiple times since we do not use locking here but it's not critical for now
				// as alternative we could consider e.g. locking or https://github.com/alastairtree/LazyCache
				baseModels = await _catalogAPIClient.GetBasesAsync(dtmi, cancellationToken);
				_baseModels.GetOrAdd(dtmi, baseModels);
			}
			return baseModels;
		}
	}
}
