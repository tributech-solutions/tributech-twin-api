using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public interface IRelationshipRepository {
		public Task<Relationship> CreateRelationshipAsync(Relationship relationship);
		public Task<Relationship> UpsertRelationshipAsync(Relationship relationship);
		public Task DeleteRelationshipAsync(Guid relationshipId);
		public Task<Relationship> GetRelationshipAsync(Guid relationshipId);
		public Task<PaginatedResponse<Relationship>> GetRelationshipsPaginatedAsync(int pageNumber, int pageSize);
		public Task<IEnumerable<Relationship>> GetOutgoingRelationshipsAsync(Guid twinId);
		public Task<IEnumerable<Relationship>> GetIncomingRelationshipsAsync(Guid twinId);
		
	}
}
