using System;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Application.Infrastructure {
	public interface IRelationshipRepository {
		public Task<Relationship> CreateRelationshipAsync(Relationship relationship);
		public Task<Relationship> UpsertRelationshipAsync(Relationship relationship);
		public Task<Relationship> DeleteRelationshipAsync(Guid relationshipId);
		public Task<Relationship> GetRelationshipAsync(Guid relationshipId);
	}
}
