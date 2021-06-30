using System;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public interface ITwinRepository {
		public Task<DigitalTwin> CreateTwinAsync(DigitalTwin twin);
		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin);
		public Task<DigitalTwin> DeleteTwinAsync(Guid twinId);
		public Task<DigitalTwin> GetTwinAsync(Guid twinId);
		public Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginatedAsync(uint pageNumber, uint pageSize);
		public Task<PaginatedResponse<DigitalTwin>> GetTwinsByModelPaginatedAsync(string dtmi, uint pageNumber, uint pageSize);
	}
}
