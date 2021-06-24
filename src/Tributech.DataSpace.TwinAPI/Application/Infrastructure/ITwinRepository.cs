using System;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Application.Model;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Application.Infrastructure {
	public interface ITwinRepository {
		public Task<DigitalTwin> CreateTwinAsync(DigitalTwin twin);
		public Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin);
		public Task<DigitalTwin> DeleteTwinAsync(Guid twinId);
		public Task<DigitalTwin> GetTwin(Guid twinId);
		public Task<PaginatedResponse<DigitalTwin>> GetTwinsPaginated(uint pageNumber, uint pageSize);
		public Task<Relationship> CreateRelationshipAsync(Relationship relationship);
		public Task<Relationship> DeleteRelationshipAsync(Guid relationshipId);
	}
}
