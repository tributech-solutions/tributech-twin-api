using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tributech.DataSpace.TwinAPI.Application.Schema {

	/// <summary>
	/// Service to manage schema/models (basd on data from Catalog-API).
	/// </summary>
	public interface ISchemaService {

		/// <summary>
		/// Get base model(s) of a model.
		/// </summary>
		/// <param name="dtmi">The model identifier (dtmi).</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The model identifiers (dtmi) of the base models.</returns>
		public Task<IEnumerable<string>> GetBaseModels(string dtmi, CancellationToken cancellationToken = default);
	}
}
