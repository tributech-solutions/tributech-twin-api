using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	/// <summary>
	/// Manage digital twin graph.
	/// </summary>
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class GraphController : ControllerBase {
		private readonly ILogger<GraphController> _logger;
		private readonly ITwinService _twinService;
		private readonly IRelationshipRepository _relRepository;

		public GraphController(ILogger<GraphController> logger, ITwinService twinService, IRelationshipRepository relRepository) {
			_logger = logger;
			_twinService = twinService;
			_relRepository = relRepository;
		}

		/// <summary>
		/// Upsert digital twin graph.
		/// </summary>
		/// <param name="graph">The twin graph.</param>
		/// <returns>The created/update twin graph.</returns>
		[HttpPut, HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TwinGraph))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<ActionResult> UpsertTwinGraph([FromBody] TwinGraphFile graph) {
			var _twins = graph?.Graph?.DigitalTwins;
			var _relationships = graph?.Graph?.Relationships;
			var t = await Task.WhenAll(_twins?.Select(twin => _twinService.UpsertTwinAsync(twin)) ?? Enumerable.Empty<Task<DigitalTwin>>());
			var r = await Task.WhenAll(_relationships?.Select(rel => _relRepository.CreateRelationshipAsync(rel)) ?? Enumerable.Empty<Task<Relationship>>());

			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = t.ToExpandoObject(),
				Relationships = r.ToExpandoObject()
			});
		}
	}
}
