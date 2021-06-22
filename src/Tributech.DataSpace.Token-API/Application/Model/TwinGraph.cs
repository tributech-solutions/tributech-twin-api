using System.Text.Json.Serialization;

namespace Tributech.DataSpace.TwinAPI.Application.Model {
	public class TwinGraph {
		[JsonPropertyName("digitalTwinsFileInfo")]
		public TwinGraphFileInfo FileInfo { get; set; }

		[JsonPropertyName("digitalTwins")]
		public DigitalTwin[] DigitalTwins { get; set; }

		[JsonPropertyName("relationships")]
		public Relationship[] Relationships { get; set; }
	}

	public class TwinGraphFileInfo {
		[JsonPropertyName("fileVersion")]
		public string FileVersion { get; set; }

	}
}
