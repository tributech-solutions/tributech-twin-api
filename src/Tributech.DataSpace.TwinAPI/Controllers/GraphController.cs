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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TwinGraph))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> UpsertTwinGraph([FromBody] TwinGraphFile graph) {
			var _twins = graph.Graph.DigitalTwins;
			var _relationships = graph.Graph.Relationships;
			var t = await Task.WhenAll(_twins.Select(twin => _twinRepository.CreateTwinAsync(twin)));
			var r = await Task.WhenAll(_relationships.Select(rel => _relRepository.CreateRelationshipAsync(rel)));


			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = t.ToExpandoObject(),
				Relationships = r.ToExpandoObject()
			});
		}
	}
}
