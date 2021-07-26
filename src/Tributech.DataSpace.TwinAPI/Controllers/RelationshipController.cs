using System;
using System.Collections.Generic;
using System.Linq;
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

		[HttpGet("/outgoing/{twinId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Relationship>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetOutgoingRelationships(Guid twinId) {
			var res = await _relRepository.GetOutgoingRelationshipsAsync(twinId);
			return Ok(res.ToExpandoObject());
		}
		
		[HttpGet("/incoming/{twinId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Relationship>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetIncomingRelationships(Guid twinId) {
			var res = await _relRepository.GetIncomingRelationshipsAsync(twinId);
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Get all twin relationships.
		/// </summary>
		/// <param name="pageNumber">The page number. Default:1</param>
		/// <param name="pageSize">The page size. Default:100</param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<Relationship>))]
		public async Task<IActionResult> GetAllRelationships([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			var results = await _relRepository.GetRelationshipsPaginatedAsync(pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(results.TotalElements, results.Content.Select(t => t.ToExpandoObject())));
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
