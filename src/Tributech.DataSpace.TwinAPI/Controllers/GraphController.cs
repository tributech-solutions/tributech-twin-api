using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class GraphController : ControllerBase {
		private readonly ILogger<GraphController> _logger;

		public GraphController(ILogger<GraphController> logger) {
			_logger = logger;
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
		public void UpsertTwinGraph([FromBody] TwinGraph graph) {
		}

		[HttpDelete("{dtid}")]
		public void DeleteTwinGraph(Guid rootTwinDtId) {
		}
	}
}
