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
		public BaseDigitalTwin GetRelationship(Guid relationshipId) {
			return null;
		}

		[HttpPost]
		public BaseDigitalTwin CreateRelationship([FromBody] BasicRelationship relationship) {
			return null;
		}

		[HttpPut("{relationshipId}")]
		public void UpsertRelationship(Guid dtid, [FromBody] BasicRelationship relationship) {
		}

		[HttpDelete("{relationshipId}")]
		public void Delete(Guid relationshipId) {
		}
	}
}
