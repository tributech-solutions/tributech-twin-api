using Tributech.DataSpace.TwinAPI.Application.Exceptions;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Infrastructure.Repository;
using Tributech.Dsk.CatalogApi.Client;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Application {
	public class TwinService : ITwinService {
		private readonly ITwinRepository _twinRepository;
		private readonly IRelationshipRepository _relRepository;
		private readonly CatalogApiClient _catalogApiClient;

		public TwinService(ITwinRepository twinRepository, IRelationshipRepository relRepository, CatalogApiClient catalogApiClient) {
			_twinRepository = twinRepository;
			_relRepository = relRepository;
			_catalogApiClient = catalogApiClient;
		}

		public async Task<DigitalTwin> UpsertTwinAsync(DigitalTwin twin, CancellationToken cancellationToken = default) {
			SchemaValidationError validationResult = await _catalogApiClient.ValidationController_validateInstanceAsync(twin.MapToCatalogApiModel(), cancellationToken);
			if (!validationResult.Success) {
				throw new InstanceValidationException(validationResult.Errors.MapToModel());
			}

			twin = await UpsertTwinInternalAsync(twin, cancellationToken);

			return twin;
		}

		public async Task<TwinGraph> UpsertTwinGraph(TwinGraph twinGraph, CancellationToken cancellationToken = default) {
			SchemaValidationError validationResult = await _catalogApiClient.ValidationController_validateGraphAsync(twinGraph.MapToCatalogApiModel(), cancellationToken);
			if (!validationResult.Success) {
				throw new InstanceValidationException(validationResult.Errors.MapToModel());
			}

			DigitalTwin[] twins = await Task.WhenAll(twinGraph?.DigitalTwins?.Select(twin => UpsertTwinInternalAsync(twin, cancellationToken)) ?? Enumerable.Empty<Task<DigitalTwin>>());
			DSK.Twin.Core.Implementation.Api.Relationship[] relationships = await Task.WhenAll(twinGraph?.Relationships?.Select(rel => _relRepository.CreateRelationshipAsync(rel)) ?? Enumerable.Empty<Task<DSK.Twin.Core.Implementation.Api.Relationship>>());

			return new TwinGraph {
				DigitalTwins = twins,
				Relationships = relationships
			};
		}

		private async Task<DigitalTwin> UpsertTwinInternalAsync(DigitalTwin twin, CancellationToken cancellationToken) {
			return await _twinRepository.UpsertTwinAsync(twin, cancellationToken);
		}
	}
}
