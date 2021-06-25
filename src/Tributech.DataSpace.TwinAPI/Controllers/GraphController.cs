using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class GraphController : ControllerBase {
		private readonly ILogger<GraphController> _logger;
		private readonly ITwinRepository _twinRepository;

		public GraphController(ILogger<GraphController> logger, ITwinRepository twinRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
		}

		[HttpGet]
		public IEnumerable<TwinGraph> GetAllSubgraphs() {
			return null;
		}

		[HttpGet("{dtid}")]
		public TwinGraph Get(Guid rootTwinDtId) {
			return null;
		}

		[HttpPut, HttpPost]
		public ActionResult UpsertTwinGraph([FromBody] TwinGraphFile graph) {
			var _twins = graph.Graph.DigitalTwins;
			var _relationships = graph.Graph.Relationships;
			var t = _twins.Select(async twin => await _twinRepository.CreateTwinAsync(twin));
			var r = _relationships.Select(async rel => await _twinRepository.CreateRelationshipAsync(rel));

			return Ok(new { Twins = t.Count(), Relationships = r.Count() });
		}

		[HttpDelete("{dtid}")]
		public void DeleteTwinGraph(Guid rootTwinDtId) {
		}
	}
}
