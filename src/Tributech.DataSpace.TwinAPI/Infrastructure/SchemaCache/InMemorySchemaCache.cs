using System;
using Microsoft.Extensions.Caching.Memory;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.SchemaCache
{
    public class InMemorySchemaCache: ISchemaCache
    {
        private readonly MemoryCache _memoryCache;

        public InMemorySchemaCache() {
            var opt = Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions());
            _memoryCache = new MemoryCache(opt);
        }

		public string Add(Schema modelSchema) {
			if (_memoryCache.TryGetValue(modelSchema?.ModelId, out var unused)) {
				throw new Exception("Model already exists");
			}

			_memoryCache.Set(modelSchema.ModelId, modelSchema);
			return modelSchema.ModelId;
		}

		public bool TryGet(string modelId, out Schema modelSchema) {
			return _memoryCache.TryGetValue(modelId, out modelSchema);
		}
	}
}
