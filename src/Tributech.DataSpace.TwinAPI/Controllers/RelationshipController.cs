using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetRelationship(Guid relationshipId) {
			var res = await _relRepository.GetRelationshipAsync(relationshipId);
			return Ok(res.ToExpandoObject());
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> CreateRelationship([FromBody] Relationship relationship) {
			var res = await _relRepository.CreateRelationshipAsync(relationship);
			return Ok(res.ToExpandoObject());
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpsertRelationship([FromBody] Relationship relationship) {
			var res = await _relRepository.UpsertRelationshipAsync(relationship);
			return Ok(res.ToExpandoObject());
		}

		[HttpDelete("{relationshipId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid relationshipId) {
			var res = await _relRepository.DeleteRelationshipAsync(relationshipId);
			return Ok(res.ToExpandoObject());
		}
	}
}
