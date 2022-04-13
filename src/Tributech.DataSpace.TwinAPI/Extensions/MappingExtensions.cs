using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Extensions {
	public static class MappingExtensions {

		public static Dsk.CatalogApi.Client.BaseDigitalTwin MapToCatalogApiModel(this DigitalTwin twin) {
			return new Dsk.CatalogApi.Client.BaseDigitalTwin {
				DtId = twin.Id.ToString(),
				Etag = twin.ETag,
				Metadata = twin.Metadata.MapToCatalogApiModel(),
				AdditionalProperties = twin.Properties
			};
		}

		public static Dsk.CatalogApi.Client.BasicRelationship MapToCatalogApiModel(this Relationship relationship) {
			return new Dsk.CatalogApi.Client.BasicRelationship {
				RelationshipId = relationship.Id.ToString(),
				SourceId = relationship.SourceId.ToString(),
				TargetId = relationship.TargetId.ToString(),
				RelationshipName = relationship.Name,
				Etag = relationship.ETag,
				AdditionalProperties = relationship.Properties
			};
		}

		public static Dsk.CatalogApi.Client.DigitalTwinMetadata MapToCatalogApiModel(this DigitalTwinMetadata metadata) {
			return new Dsk.CatalogApi.Client.DigitalTwinMetadata {
				Model = metadata.ModelId
			};
		}

		public static Dsk.CatalogApi.Client.DigitalTwinModel MapToCatalogApiModel(this TwinGraph twinGraph) {
			return new Dsk.CatalogApi.Client.DigitalTwinModel {
				DigitalTwins = twinGraph?.DigitalTwins?.Select(t => t.MapToCatalogApiModel()).ToList(),
				Relationships = twinGraph?.Relationships?.Select(r => r.MapToCatalogApiModel()).ToList(),
			};
		}

		public static string MapToModel(this Dsk.CatalogApi.Client.SchemaErrorObject error)
			=> error.Message;

		public static IDictionary<string, string[]> MapToModel(this ICollection<Dsk.CatalogApi.Client.SchemaErrorObject> errors) 
			=> errors.GroupBy(error => error.InstancePath).ToDictionary(group => group.Key, group => group.Select(error => error.MapToModel()).ToArray());
	}
}
