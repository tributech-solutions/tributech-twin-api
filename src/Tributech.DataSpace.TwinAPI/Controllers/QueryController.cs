using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	/// <summary>
	/// Querying of digital twins.
	/// </summary>
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class QueryController : ControllerBase {
		private readonly ILogger<QueryController> _logger;
		private readonly IQueryRepository _queryRepository;

		public QueryController(ILogger<QueryController> logger, IQueryRepository queryRepository) {
			_logger = logger;
			_queryRepository = queryRepository;
		}

		/// <summary>
		/// Query digital twin graph using cypher query.
		/// </summary>
		/// <param name="query">The cypher query.</param>
		/// <returns>The digital twin (sub-)graph.</returns>
		[HttpPost("cypher", Name = nameof(GetTwinGraphByCypherQuery))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TwinGraph))]
		public async Task<IActionResult> GetTwinGraphByCypherQuery([FromQuery] TwinCypherQuery query) {
			TwinGraph res = await _queryRepository.GetByCypherQuery(query);
			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = res?.DigitalTwins?.ToExpandoObject() ?? Array.Empty<dynamic>(),
				Relationships = res?.Relationships?.ToExpandoObject() ?? Array.Empty<dynamic>()
			});
		}

		/// <summary>
		/// Query digital twin graph by criterias.
		/// </summary>
		/// <param name="query">The graph query</param>
		/// <returns>The digital twin (sub-)graph.</returns>
		[HttpPost("subgraph", Name = nameof(GetTwinGraphByQuery))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TwinGraph))]
		public async Task<IActionResult> GetTwinGraphByQuery([FromBody] TwinGraphQuery query) {
			TwinGraph res = await _queryRepository.GetSubgraph(query);
			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = res?.DigitalTwins?.ToExpandoObject() ?? Array.Empty<dynamic>(),
				Relationships = res?.Relationships?.ToExpandoObject() ?? Array.Empty<dynamic>()
			});
		}
	}
}
