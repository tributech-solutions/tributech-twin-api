﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;
using Tributech.DataSpace.TwinAPI.Application.Model;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class InstanceController : ControllerBase {
		private readonly ILogger<InstanceController> _logger;
		private readonly ITwinRepository _twinRepository;

		public InstanceController(ILogger<InstanceController> logger, ITwinRepository twinRepository) {
			_logger = logger;
			_twinRepository = twinRepository;
		}

		[HttpGet]
		public Task<PaginatedResponse<DigitalTwin>> GetAllTwins() {
			return _twinRepository.GetTwinsPaginated(0, 100);
		}

		[HttpGet("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTwin(Guid dtid) {
			var twin = await _twinRepository.GetTwin(dtid);

			return Content(twin.GetExpanded(), "application/json");
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTwin([FromBody] DigitalTwin twin) {
			var _twin = await _twinRepository.CreateTwinAsync(twin);
			return Content(_twin.GetExpanded(), "application/json");

		}

		[HttpPut("{dtid}")]
				[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpsertTwin(Guid dtid, [FromBody] DigitalTwin twin) {
			var _twin = await _twinRepository.UpsertTwinAsync(twin);
			return Content(_twin.GetExpanded(), "application/json");

		}

		[HttpDelete("{dtid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalTwin))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid dtid) {
			var twin = await _twinRepository.DeleteTwinAsync(dtid);
			return Content(twin.GetExpanded(), "application/json");

		}
	}
}
