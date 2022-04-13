using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tributech.DataSpace.TwinAPI.Application;
using Tributech.DataSpace.TwinAPI.Application.Exceptions;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

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

		public GraphController(ILogger<GraphController> logger, ITwinService twinService) {
			_logger = logger;
			_twinService = twinService;
		}

		/// <summary>
		/// Upsert digital twin graph.
		/// </summary>
		/// <param name="graph">The twin graph.</param>
		/// <returns>The created/updated twin graph.</returns>
		[HttpPut(Name = nameof(UpsertTwinGraph))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TwinGraph))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<ActionResult> UpsertTwinGraph([FromBody] TwinGraphFile graph) {
			TwinGraph result;
			try {
				result = await _twinService.UpsertTwinGraph(graph?.Graph);
			}
			catch (InstanceValidationException ex) {
				// prefix for sub proprty using JSON pointer based syntax
				ModelState.AddModelErrors(ex.Errors, $"/{TwinGraphFile.GraphJsonPropertyName}");
				return BadRequest(ModelState);
			}

			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = result.DigitalTwins.ToExpandoObject(),
				Relationships = result.Relationships.ToExpandoObject()
			});
		}
	}
}
