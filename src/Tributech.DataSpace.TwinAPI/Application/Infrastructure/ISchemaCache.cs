using System;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Application.Infrastructure {
	public interface ISchemaCache {
		string Add(Schema schema);

		bool TryGet(string modelId, out Schema schema);
	}
}
