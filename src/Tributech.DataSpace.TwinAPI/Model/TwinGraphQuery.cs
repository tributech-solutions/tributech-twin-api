namespace Tributech.DataSpace.TwinAPI.Model {
#nullable enable
	public class TwinGraphQuery {
		public string? StartNodeDtId { get; set; }
		// SOURCES>,STREAMS>,SINKS>
		public string? RelationshipFilter { get; set; }
		// -{{BLACKLIST}}
		// +{{WHITELIST}}
		// /{{TERMINATION}}
		// >{{END_NODE}}
		public string? LabelFilter { get; set; }
		public uint MaxDepth { get; set; } = 3;
	}
#nullable disable
}
