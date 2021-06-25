﻿using Newtonsoft.Json;

namespace Tributech.DataSpace.TwinAPI.Application.Model {
	public class TwinGraph {

		[JsonProperty("digitalTwins")]
		public DigitalTwin[] DigitalTwins { get; set; }

		[JsonProperty("relationships")]
		public Relationship[] Relationships { get; set; }
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
