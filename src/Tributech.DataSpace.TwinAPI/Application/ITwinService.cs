using System.Threading;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Model;

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
		/// <returns></returns>
		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken = default);
	}
}
