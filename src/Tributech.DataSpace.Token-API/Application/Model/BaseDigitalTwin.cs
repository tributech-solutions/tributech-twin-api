using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tributech.DataSpace.TwinAPI.Application.Model {

	public class BaseDigitalTwin {

		[JsonPropertyName("$dtId")]
		public Guid Id { get; set; }

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
