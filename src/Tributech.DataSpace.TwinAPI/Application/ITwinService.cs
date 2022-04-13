using Tributech.DataSpace.TwinAPI.Application.Exceptions;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Application {

	/// <summary>
	/// Service to manage twin instances.
	/// </summary>
	public interface ITwinService {

		/// <summary>
		/// Insert/update twin instance.
		/// </summary>
		/// <param name="twin">The twin instance</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The created/updated twin.</returns>
		/// <exception cref="InstanceValidationException">Validation errors for twin/relationship instances.</exception>
		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upsert digital twin graph.
		/// </summary>
		/// <param name="twinGraph">The twin graph.</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The created/updated twin graph.</returns>
		/// <exception cref="InstanceValidationException">Validation errors for twin/relationship instances.</exception>
		public Task<TwinGraph> UpsertTwinGraph(TwinGraph twinGraph, CancellationToken cancellationToken = default);
	}
}
