using System;
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
	public class TwinsController : ControllerBase {
		private readonly ILogger<TwinsController> _logger;
		private readonly ITwinRepository _twinRepository;

		public TwinsController(ILogger<TwinsController> logger, ITwinRepository twinRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<DigitalTwin>))]
		public async Task<IActionResult> GetAllTwins([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			var results = await _twinRepository.GetTwinsPaginatedAsync(pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(results.TotalElements, results.Content.Select(t => t.ToExpandoObject())));
		}


		[HttpGet("/model/{dtmi}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<DigitalTwin>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTwinsByModel(string dtmi, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			var paginated = await _twinRepository.GetTwinsByModelPaginatedAsync(dtmi, pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(paginated.TotalElements, paginated.Content.Select(t => t.ToExpandoObject())));
		}

		[HttpGet("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTwin(Guid dtid) {
			var twin = await _twinRepository.GetTwinAsync(dtid);
			return Ok(twin.ToExpandoObject());
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTwin([FromBody] DigitalTwin twin) {
			var _twin = await _twinRepository.CreateTwinAsync(twin);
			return Ok(_twin.ToExpandoObject());

		}

		[HttpPut("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpsertTwin(Guid dtid, [FromBody] DigitalTwin twin) {
			var _twin = await _twinRepository.UpsertTwinAsync(twin);
			return Ok(_twin.ToExpandoObject());
		}

		[HttpDelete("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid dtid) {
			var twin = await _twinRepository.DeleteTwinAsync(dtid);
			return Ok(twin.ToExpandoObject());
		}
	}
}
