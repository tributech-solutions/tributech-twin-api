using System;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public interface ITwinRepository {
		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin);
		public Task DeleteTwinAsync(Guid twinId);
		public Task<DigitalTwin> GetTwinAsync(Guid twinId);
		public Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginatedAsync(int pageNumber, int pageSize);
		public Task<PaginatedResponse<DigitalTwin>> GetTwinsByModelPaginatedAsync(string dtmi, int pageNumber, int pageSize);
	}
}
