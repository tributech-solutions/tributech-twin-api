using System;

namespace Tributech.DataSpace.TwinAPI.Model {
	public class TwinDto {
		public Guid Id { get; set; }

		public string ETag { get; set; }

		public string ModelId { get; set; }

		// public Dictionary<string, string> Properties { get; set; }
	}
}
