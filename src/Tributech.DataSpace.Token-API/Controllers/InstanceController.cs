using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tributech.DataSpace.TwinAPI.Model;

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
		public IEnumerable<BaseDigitalTwin> Get() {
			return new BaseDigitalTwin[] {
			new BaseDigitalTwin() {
					Id = "tets",
					ETag = "tetet",
					Properties = new Dictionary<string, JsonElement> (),
					Metadata = new DigitalTwinMetadata() {
						ModelId = "dtmi:io:tributech:test;1"
					}
				}
			};
		}

		// GET api/<InstanceController>/5
		[HttpGet("{dtid}")]
		public string Get(int dtid) {
			return "value";
		}

		// POST api/<InstanceController>
		[HttpPost]
		public void Post([FromBody] BaseDigitalTwin twin) {
			string dictionaryString = "{";
			foreach (KeyValuePair<string, JsonElement> keyValues in twin?.Properties) {
				dictionaryString += keyValues.Key + " : " + keyValues.Value.ToString() + ", ";
			}
			_logger.LogWarning(dictionaryString.TrimEnd(',', ' ') + "}");
		}

		// PUT api/<InstanceController>/5
		[HttpPut("{dtid}")]
		public void Put(int dtid, [FromBody] BaseDigitalTwin twin) {
		}

		// DELETE api/<InstanceController>/5
		[HttpDelete("{dtid}")]
		public void Delete(int dtid) {
		}
	}
}
