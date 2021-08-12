using System.ComponentModel.DataAnnotations;

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

	/// <summary>
	/// Query twins and relationships using cypher query.
	/// The result must be projected to lists named "nodes" and "relationships" to be returned.
	/// </summary>
	public class TwinCypherQuery {

		/// <summary>
		/// Cypher query <a href="https://neo4j.com/docs/cypher-manual/current/clauses/match/" target="_blank">MATCH</a> part.
		/// e.g. "(twin:Twin)", "(stream:Twin:BaseStreamTributechIoV1)", "(stream:Twin:BaseStreamTributechIoV1)-[relationship:Options]->(option:Twin:BaseOptionsTributechIoV1)",...
		/// </summary>
		/// <example>(stream:Twin:BaseStreamTributechIoV1)-[relationship:Options]->(option:Twin:BaseOptionsTributechIoV1)</example>
		[Required]
		public string Match { get; set; }

		/// <summary>
		/// (Optional) Cypher query <a href="https://neo4j.com/docs/cypher-manual/current/clauses/where/" target="_blank">WHERE</a> part.
		/// e.g. "stream.ValueMetadataId = '6e02502-bfc5-4182-aa3b-891965020e14'", "stream.MerkleTreeDepth IS NOT NULL", "option:PersistenceOptionsTributechIoV1",...
		/// </summary>
		/// <example>option:PublishOptionsTributechIoV1 OR option:PersistenceOptionsTributechIoV1</example>
		public string Where { get; set; }

		/// <summary>
		/// Cypher query <a href="https://neo4j.com/docs/cypher-manual/current/clauses/with/" target="_blank">WITH</a> part.
		/// The result must be projected to lists named "nodes" and "relationships" to be returned which can be done at the WITH part.
		/// To return an empty list for one of them you can use "[] AS relationships".
		/// e.g. "stream AS nodes, [] AS relationships", "collect(distinct stream) + collect(distinct option) AS nodes, collect(relationship) AS relationships"),...
		/// </summary>
		/// <example>collect(distinct stream) + collect(distinct option) AS nodes, collect(relationship) AS relationships</example>
		[Required]
		public string With { get; set; }
	}
}
