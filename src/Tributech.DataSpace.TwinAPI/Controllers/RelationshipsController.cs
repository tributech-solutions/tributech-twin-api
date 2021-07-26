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

	/// <summary>
	/// Manage digital twin relationships.
	/// </summary>
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class RelationshipsController : ControllerBase {
		private readonly ILogger<RelationshipsController> _logger;
		private readonly IRelationshipRepository _relRepository;

		public RelationshipsController(ILogger<RelationshipsController> logger, IRelationshipRepository relRepository) {
			_logger = logger;
			_relRepository = relRepository;
		}

		/// <summary>
		/// Get all outgoing relationships for a digital twin.
		/// </summary>
		/// <param name="dtId">The digital twin identifier.</param>
		/// <returns>List of outgoing relationships of the digital twin.</returns>
		[HttpGet("/outgoing/{dtId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Relationship>))]
		public async Task<IActionResult> GetOutgoingRelationships(Guid dtId) {
			IEnumerable<Relationship> res = await _relRepository.GetOutgoingRelationshipsAsync(dtId);
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Get all incoming relationships for a digital twin.
		/// </summary>
		/// <param name="dtId">The digital twin identifier.</param>
		/// <returns>List of incoming relationships of the digital twin.</returns>
		[HttpGet("/incoming/{dtId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Relationship>))]
		public async Task<IActionResult> GetIncomingRelationships(Guid dtId) {
			IEnumerable<Relationship> res = await _relRepository.GetIncomingRelationshipsAsync(dtId);
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Get all relationships between digital twins.
		/// </summary>
		/// <param name="pageNumber">The page number. Default:1</param>
		/// <param name="pageSize">The page size. Default:100</param>
		/// <returns>List of digital twins relationships.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<Relationship>))]
		public async Task<IActionResult> GetAllRelationships([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			PaginatedResponse<Relationship> results = await _relRepository.GetRelationshipsPaginatedAsync(pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(results.TotalElements, results.Content.Select(t => t.ToExpandoObject())));
		}

		/// <summary>
		/// Get single relationship between digital twins by relationship id.
		/// </summary>
		/// <param name="relationshipId">The relationship identifier.</param>
		/// <returns>The relationship.</returns>
		[HttpGet("{relationshipId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetRelationship(Guid relationshipId) {
			Relationship res = await _relRepository.GetRelationshipAsync(relationshipId);
			if (res == null) {
				return NotFound();
			}
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Create new relationship between digital twins.
		/// Currently no checks regarding existence of referenced twins is made!
		/// </summary>
		/// <param name="relationship">The relationship.</param>
		/// <returns>The relationship.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<IActionResult> CreateRelationship([FromBody] Relationship relationship) {
			Relationship res = await _relRepository.CreateRelationshipAsync(relationship);
			if (res == null) {
				ModelState.AddModelError("", "Invalid request. Please check e.g. existence of referenced twins.");
				return BadRequest(ModelState);
			}
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Create or update relationship between digital twins.
		/// urrently no checks regarding existence of referenced twins is made!
		/// </summary>
		/// <param name="relationshipId">The relationship identifier.</param>
		/// <param name="relationship">The relationship.</param>
		/// <returns>The relationship.</returns>
		[HttpPut("{relationshipId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Relationship))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<IActionResult> UpsertRelationship(Guid relationshipId, [FromBody] Relationship relationship) {
			relationship.Id = relationshipId;
			Relationship res = await _relRepository.UpsertRelationshipAsync(relationship);
			if (res == null) {
				ModelState.AddModelError("", "Invalid request. Please check e.g. existence of referenced twins.");
				return BadRequest(ModelState);
			}
			return Ok(res.ToExpandoObject());
		}

		/// <summary>
		/// Delete relationship of digital twins (if exists).
		/// </summary>
		/// <param name="relationshipId">The relationship identifier.</param>
		[HttpDelete("{relationshipId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Delete(Guid relationshipId) {
			await _relRepository.DeleteRelationshipAsync(relationshipId);
			return Ok();
		}
	}
}
