using System.Collections.Generic;
using Newtonsoft.Json;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Model {
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
		public const string GraphJsonPropertyName = "digitalTwinsGraph";

		[JsonProperty("digitalTwinsFileInfo")]
		public TwinGraphFileInfo FileInfo { get; set; }

		[JsonProperty(GraphJsonPropertyName)]
		public TwinGraph Graph { get; set; }
	}
}
