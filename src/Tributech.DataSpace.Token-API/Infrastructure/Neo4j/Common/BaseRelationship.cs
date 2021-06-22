using System.Text.Json.Serialization;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j.Common {
	public class BaseRelationship : IRelationship {
		[JsonIgnore]
		public string Type { get; private set; }

		/// <summary>
		/// Type/Name of the relationship
		/// </summary>
		/// <param name="type"></param>
		public BaseRelationship(string type) {
			Type = type;
		}
	}
}
