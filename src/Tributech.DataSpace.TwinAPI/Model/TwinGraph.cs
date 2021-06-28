using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tributech.DataSpace.TwinAPI.Model {
	public class TwinGraph {

		[JsonProperty("digitalTwins")]
		public IEnumerable<DigitalTwin> DigitalTwins { get; set; }

		[JsonProperty("relationships")]
		public IEnumerable<Relationship> Relationships { get; set; }
	}

	public class TwinGraph<T, R> {

		[JsonProperty("digitalTwins")]
		public IEnumerable<T> DigitalTwins { get; set; }

		[JsonProperty("relationships")]
		public IEnumerable<R> Relationships { get; set; }
	}

	public class TwinGraphFileInfo {
		[JsonProperty("fileVersion")]
		public string FileVersion { get; set; }
	}

	public class TwinGraphFile {
		[JsonProperty("digitalTwinsFileInfo")]
		public TwinGraphFileInfo FileInfo { get; set; }

		[JsonProperty("digitalTwinsGraph")]
		public TwinGraph Graph { get; set; }
	}
}
