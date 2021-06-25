using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class QueryController : ControllerBase {
		private readonly ILogger<QueryController> _logger;
		private readonly ITwinRepository _twinRepository;

		public QueryController(ILogger<QueryController> logger, ITwinRepository twinRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
		}

		[HttpGet("{dtmi}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSubgraphByQuery([FromBody] TwinInstanceMetaQuery query) {
			return null;
		}
	}
}
