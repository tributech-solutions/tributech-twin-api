using System;

namespace Tributech.DataSpace.TwinAPI.Model {
	public class RelationshipDto {
		public Guid Id { get; set; }
		public string ETag { get; set; }
		public Guid SourceId { get; set; }
		public Guid TargetId { get; set; }
		public string Name { get; set; }
		// public Dictionary<string, string> Properties { get; set; }
	}
}
