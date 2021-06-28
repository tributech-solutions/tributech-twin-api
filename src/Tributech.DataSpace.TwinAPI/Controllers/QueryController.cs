using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class QueryController : ControllerBase {
		private readonly ILogger<QueryController> _logger;
		private readonly IQueryRepository _queryRepository;

		public QueryController(ILogger<QueryController> logger, IQueryRepository queryRepository) {
			_logger = logger;
			_queryRepository = queryRepository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSubgraphByQuery([FromBody] TwinGraphQuery query) {
			var res = await _queryRepository.GetSubgraph(query);
			return Ok(new TwinGraph<dynamic, dynamic>() {
				DigitalTwins = res.DigitalTwins.ToExpandoObject(),
				Relationships = res.Relationships.ToExpandoObject()
			});
		}
	}
}
