using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.SchemaCache {
	public interface ISchemaCache {
		string Add(Schema schema);

		bool TryGet(string modelId, out Schema schema);
	}
}
