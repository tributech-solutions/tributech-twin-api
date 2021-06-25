using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Controllers {

	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	[ApiController]
	public class InstanceController : ControllerBase {
		private readonly ILogger<InstanceController> _logger;

		public InstanceController(ILogger<InstanceController> logger) {
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<BaseDigitalTwin> GetAllTwins() {
			return new BaseDigitalTwin[] {
			new BaseDigitalTwin() {
					Id = new Guid(),
					ETag = "tetet",
					Properties = new Dictionary<string, JsonElement> (),
					Metadata = new DigitalTwinMetadata() {
						ModelId = "dtmi:io:tributech:test;1"
					}
				}
			};
		}

		[HttpGet("{dtid}")]
		public BaseDigitalTwin GetTwin(Guid dtid) {
			return null;
		}

		[HttpPost]
		public BaseDigitalTwin AddTwin([FromBody] BaseDigitalTwin twin) {
			return null;
		}

		[HttpPut("{dtid}")]
		public void UpsertTwin(Guid dtid, [FromBody] BaseDigitalTwin twin) {
		}

		[HttpDelete("{dtid}")]
		public void Delete(Guid dtid) {
		}
	}
}
