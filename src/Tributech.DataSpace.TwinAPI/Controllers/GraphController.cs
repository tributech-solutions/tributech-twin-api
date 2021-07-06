using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class GraphController : ControllerBase {
		private readonly ILogger<GraphController> _logger;
		private readonly ITwinRepository _twinRepository;
		private readonly IRelationshipRepository _relRepository;

		public GraphController(ILogger<GraphController> logger, ITwinRepository twinRepository, IRelationshipRepository relRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
			_relRepository = relRepository;
		}

		[HttpPut, HttpPost]
		public ActionResult UpsertTwinGraph([FromBody] TwinGraphFile graph) {
			var _twins = graph.Graph.DigitalTwins;
			var _relationships = graph.Graph.Relationships;
			var t = _twins.Select(async twin => await _twinRepository.CreateTwinAsync(twin)).ToArray();
			var r = _relationships.Select(async rel => await _relRepository.CreateRelationshipAsync(rel)).ToArray();

			return Ok(new { Twins = t.Count(), Relationships = r.Count() });
		}
	}
}
