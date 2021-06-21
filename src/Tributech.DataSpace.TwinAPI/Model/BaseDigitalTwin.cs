using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tributech.DataSpace.TwinAPI.Model {

	public record BaseDigitalTwin {

		[JsonPropertyName("$dtId")]
		public string Id { get; set; }

		[JsonPropertyName("$etag")]
		public string ETag { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement> Properties { get; set; }

		[JsonPropertyName("$metadata")]
		public DigitalTwinMetadata Metadata { get; set; }
	}


	public record DigitalTwinMetadata {
		public string ModelId { get; set; }

	}
}
