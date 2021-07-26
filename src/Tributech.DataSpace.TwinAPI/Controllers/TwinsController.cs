using System;
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
	/// Manage digital twin instances.
	/// </summary>
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class TwinsController : ControllerBase {
		private readonly ILogger<TwinsController> _logger;
		private readonly ITwinRepository _twinRepository;
		private readonly ITwinService _twinService;

		public TwinsController(ILogger<TwinsController> logger, ITwinRepository twinRepository, ITwinService twinService) {
			_logger = logger;
			_twinRepository = twinRepository;
			_twinService = twinService;
		}

		/// <summary>
		/// Get all digital twins.
		/// </summary>
		/// <param name="pageNumber">The page number. Default:1</param>
		/// <param name="pageSize">The page size. Default:100</param>
		/// <returns>Paged result list of digital twins.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<DigitalTwin>))]
		public async Task<IActionResult> GetAllTwins([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			PaginatedResponse<DigitalTwin> results = await _twinRepository.GetTwinsPaginatedAsync(pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(results.TotalElements, results.Content.Select(t => t.ToExpandoObject())));
		}

		/// <summary>
		/// Get all digital twins of certain model type.
		/// </summary>
		/// <param name="dtmi">The digital twin model identifier (DTMI).</param>
		/// <param name="pageNumber">The page number. Default:1</param>
		/// <param name="pageSize">The page size. Default:100</param>
		/// <returns>Paged result list of digital twins.</returns>
		[HttpGet("/model/{dtmi}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<DigitalTwin>))]
		public async Task<IActionResult> GetTwinsByModel(string dtmi, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100) {
			PaginatedResponse<DigitalTwin> paginated = await _twinRepository.GetTwinsByModelPaginatedAsync(dtmi, pageNumber, pageSize);
			return Ok(new PaginatedResponse<object>(paginated.TotalElements, paginated.Content.Select(t => t.ToExpandoObject())));
		}

		/// <summary>
		/// Get single digital twin by id.
		/// </summary>
		/// <param name="dtid">The digital twin identifier.</param>
		/// <returns>The digital twin.</returns>
		[HttpGet("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTwin(Guid dtid) {
			DigitalTwin twin = await _twinRepository.GetTwinAsync(dtid);
			if (twin == null) {
				return NotFound();
			}
			return Ok(twin.ToExpandoObject());
		}

		/// <summary>
		/// Create digital twin.
		/// </summary>
		/// <param name="twin">The digital twin.</param>
		/// <returns>The digital twin.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<IActionResult> AddTwin([FromBody] DigitalTwin twin) {
			twin = await _twinService.UpsertTwinAsync(twin);
			return Ok(twin.ToExpandoObject());
		}

		/// <summary>
		/// Create or update digital twin.
		/// </summary>
		/// <param name="dtid">The digital twin identifier.</param>
		/// <param name="twin">The digital twin.</param>
		/// <returns>The digital twin.</returns>
		[HttpPut("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
		public async Task<IActionResult> UpsertTwin(Guid dtid, [FromBody] DigitalTwin twin) {
			twin.Id = dtid;
			twin = await _twinService.UpsertTwinAsync(twin);
			return Ok(twin.ToExpandoObject());
		}

		/// <summary>
		/// Delete digital twin (if exists) and all its relationsips.
		/// </summary>
		/// <param name="dtid">The digital twin identifier.</param>
		[HttpDelete("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Delete(Guid dtid) {
			await _twinRepository.DeleteTwinAsync(dtid);
			return Ok();
		}
	}
}
