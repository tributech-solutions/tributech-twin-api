using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Application.Exceptions;
using Tributech.DataSpace.TwinAPI.Application.Schema;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.DataSpace.TwinAPI.Model;
using CatalogApi = Tributech.Dsk.CatalogApi.Client;

namespace Tributech.DataSpace.TwinAPI.Application {
	public class TwinService : ITwinService {
		private readonly ITwinRepository _twinRepository;
		private readonly IRelationshipRepository _relRepository;
		private readonly CatalogApi.CatalogApiClient _catalogApiClient;
		private readonly ISchemaService _schemaService;

		public TwinService(ISchemaService schemaService, ITwinRepository twinRepository, IRelationshipRepository relRepository, CatalogApi.CatalogApiClient catalogApiClient) {
			_schemaService = schemaService;
			_twinRepository = twinRepository;
			_relRepository = relRepository;
			_catalogApiClient = catalogApiClient;
		}

		public async Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken = default) {
			CatalogApi.SchemaValidationError validationResult = await _catalogApiClient.ValidationController_validateInstanceAsync(twin.MapToCatalogApiModel(), cancellationToken);
			if (!validationResult.Success) {
				throw new InstanceValidationException(validationResult.Errors.MapToModel());
			}

			twin = await UpsertTwinInternalAsync(twin, cancellationToken);

			return twin;
		}

		public async Task<TwinGraph> UpsertTwinGraph(TwinGraph twinGraph, CancellationToken cancellationToken = default) {
			CatalogApi.SchemaValidationError validationResult = await _catalogApiClient.ValidationController_validateGraphAsync(twinGraph.MapToCatalogApiModel(), cancellationToken);
			if (!validationResult.Success) {
				throw new InstanceValidationException(validationResult.Errors.MapToModel());
			}

			DigitalTwin[] twins = await Task.WhenAll(twinGraph?.DigitalTwins?.Select(twin => UpsertTwinInternalAsync(twin)) ?? Enumerable.Empty<Task<DigitalTwin>>());
			Relationship[] relationships = await Task.WhenAll(twinGraph?.Relationships?.Select(rel => _relRepository.CreateRelationshipAsync(rel)) ?? Enumerable.Empty<Task<Relationship>>());

			return new TwinGraph {
				DigitalTwins = twins,
				Relationships = relationships
			};
		}

		private async Task<DigitalTwin> UpsertTwinInternalAsync(DigitalTwin twin, CancellationToken cancellationToken = default) {
			// add twin model type (DTMI) as label
			twin.AddLabels(twin.Metadata.ModelId.ToLabel());

			// add twin base model types (DTMI) as labels
			IEnumerable<string> baseModelTypes = await _schemaService.GetBaseModels(twin.Metadata.ModelId, cancellationToken);
			twin.AddLabels(baseModelTypes.Select(dtmi => dtmi.ToLabel()).ToArray());

			twin = await _twinRepository.UpsertTwinAsync(twin);

			return twin;
		}
	}
}
