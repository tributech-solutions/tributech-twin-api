using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class RelationshipController : ControllerBase {
		private readonly ILogger<RelationshipController> _logger;

		public RelationshipController(ILogger<RelationshipController> logger) {
			_logger = logger;
		}

		[HttpGet("{relationshipId}")]
		public DigitalTwin GetRelationship(Guid relationshipId) {
			return null;
		}

		[HttpPost]
		public DigitalTwin CreateRelationship([FromBody] Relationship relationship) {
			return null;
		}

		[HttpPut("{relationshipId}")]
		public void UpsertRelationship(Guid dtid, [FromBody] Relationship relationship) {
		}

		[HttpDelete("{relationshipId}")]
		public void Delete(Guid relationshipId) {
		}
	}
}
