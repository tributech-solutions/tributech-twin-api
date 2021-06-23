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
		private readonly ITwinRepository _twinRepository;

		public RelationshipController(ILogger<RelationshipController> logger, ITwinRepository twinRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
		}

		[HttpGet("{relationshipId}")]
		public DigitalTwin GetRelationship(Guid relationshipId) {
			return null;
		}

		[HttpPost]
		public Task<Relationship> CreateRelationship([FromBody] Relationship relationship) {
			return _twinRepository.CreateRelationshipAsync(relationship);
		}

		[HttpPut("{relationshipId}")]
		public void UpsertRelationship(Guid dtid, [FromBody] Relationship relationship) {
		}

		[HttpDelete("{relationshipId}")]
		public void Delete(Guid relationshipId) {
		}
	}
}
