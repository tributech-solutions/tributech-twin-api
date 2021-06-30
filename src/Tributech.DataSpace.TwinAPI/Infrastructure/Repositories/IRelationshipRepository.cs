using System;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public interface IRelationshipRepository {
		public Task<Relationship> CreateRelationshipAsync(Relationship relationship);
		public Task<Relationship> UpsertRelationshipAsync(Relationship relationship);
		public Task<Relationship> DeleteRelationshipAsync(Guid relationshipId);
		public Task<Relationship> GetRelationshipAsync(Guid relationshipId);
	}
}
