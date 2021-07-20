using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Application.Schema;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Application {
	public class TwinService : ITwinService {
		private readonly ITwinRepository _twinRepository;
		private readonly ISchemaService _schemaService;

		public TwinService(ISchemaService schemaService, ITwinRepository twinRepository) {
			_schemaService = schemaService;
			_twinRepository = twinRepository;
		}

		public async Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken = default) {
			// add twin model type (DTMI) as label
			twin.AddLabels(twin.Metadata.ModelId.ToLabel());

			// add twin base model types (DTMI) as labels
			IEnumerable<string> baseModelTypes = await _schemaService.GetBaseModels(twin.Metadata.ModelId, cancellationToken);
			twin.AddLabels(baseModelTypes.Select(dtmi => dtmi.ToLabel()).ToArray());

			DigitalTwin _twin = await _twinRepository.UpsertTwinAsync(twin);

			return twin;
		}
	}
}
