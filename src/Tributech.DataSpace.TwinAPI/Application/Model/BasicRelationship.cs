using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tributech.DataSpace.TwinAPI.Application.Model {
	public class BasicRelationship {

		[JsonPropertyName("$relationshipId")]
		public Guid Id { get; set; }

		[JsonPropertyName("$etag")]
		public string ETag { get; set; }

		[JsonPropertyName("$sourceId")]
		public Guid SourceId { get; set; }

		[JsonPropertyName("$targetId")]
		public Guid TargetId { get; set; }

		[JsonPropertyName("$relationshipName")]
		public string Name { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement> Properties { get; set; }
	}
}
