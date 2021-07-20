using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tributech.Dsk.Api.Clients.CatalogApi;

namespace Tributech.DataSpace.TwinAPI.Application.Schema {
	public class SchemaService : ISchemaService {
		private readonly CatalogAPIClient _catalogAPIClient;
		// schemas are immutable that's why we cache them forever and use a dict
		private readonly ConcurrentDictionary<string, ExpandedInterface> _models = new ConcurrentDictionary<string, ExpandedInterface>();

		public SchemaService(CatalogAPIClient catalogAPIClient) {
			_catalogAPIClient = catalogAPIClient;
		}

		public async Task<IEnumerable<string>> GetBaseModels(string dtmi, CancellationToken cancellationToken = default) {
			if (!_models.TryGetValue(dtmi, out ExpandedInterface model)) {
				// in a worst case retrieval could happen multiple times since we do not use locking here but it's not critical for now
				// as alternative we could consider e.g. locking or https://github.com/alastairtree/LazyCache
				model = await _catalogAPIClient.GetExpandedAsync(dtmi, cancellationToken);
				_models.GetOrAdd(dtmi, model);
			}
			return model.Bases;
		}
	}
}
