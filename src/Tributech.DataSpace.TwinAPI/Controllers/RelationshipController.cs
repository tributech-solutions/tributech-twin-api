using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class RelationshipController : ControllerBase {
		private readonly ILogger<RelationshipController> _logger;
		private readonly IRelationshipRepository _relRepository;

		public RelationshipController(ILogger<RelationshipController> logger, IRelationshipRepository relRepository) {
			_logger = logger;
			_relRepository = relRepository;
		}

		[HttpGet("{relationshipId}")]
		public Task<Relationship> GetRelationship(Guid relationshipId) {
			return _relRepository.GetRelationshipAsync(relationshipId);
		}

		[HttpPost]
		public Task<Relationship> CreateRelationship([FromBody] Relationship relationship) {
			return _relRepository.CreateRelationshipAsync(relationship);
		}

		[HttpPut]
		public Task<Relationship> UpsertRelationship([FromBody] Relationship relationship) {
			return _relRepository.UpsertRelationshipAsync(relationship);
		}

		[HttpDelete("{relationshipId}")]
		public Task<Relationship> Delete(Guid relationshipId) {
			return _relRepository.DeleteRelationshipAsync(relationshipId);
		}
	}
}
