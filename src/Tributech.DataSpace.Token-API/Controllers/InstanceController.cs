using System;
using System.Threading.Tasks;
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
		public Task<DigitalTwin> GetTwin(Guid dtid) {
			return null;
		}

		[HttpPost]
		public Task<DigitalTwin> AddTwin([FromBody] DigitalTwin twin) {
			return _twinRepository.CreateTwinAsync(twin);
		}

		[HttpPut("{dtid}")]
		public void UpsertTwin(Guid dtid, [FromBody] DigitalTwin twin) {
		}

		[HttpDelete("{dtid}")]
		public void Delete(Guid dtid) {
		}
	}
}
