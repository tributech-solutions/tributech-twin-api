namespace Tributech.DataSpace.TwinAPI.Application.Model {
#nullable enable
	public class TwinInstanceMetaQuery {
		public string? StartNodeModelId { get; set; }
		public string[]? IncludeRelationships { get; set; }
		public string[]? ExcludeRelationships { get; set; }
		public string[]? IncludeModelIds { get; set; }
		public string[]? ExcludeModelIds { get; set; }
		public uint MaxDepth { get; set; } = 3;
	}
#nullable disable
}
