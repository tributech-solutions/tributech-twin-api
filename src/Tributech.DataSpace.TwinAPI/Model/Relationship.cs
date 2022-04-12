using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Model {
	
	public class RelationshipNode {
		public RelationshipNode() {
			Properties = new Dictionary<string, object>();
		}
		public Guid Id { get; set; }
		public string ETag { get; set; }
		public string SourceId { get; set; }
		public string TargetId { get; set; }
		public string Name { get; set; }

		[JsonExtensionData]
		public IDictionary<string, object> Properties;
	}

	public static class RelationshipExtensions {
		public static Relationship MapToRelationship(this RelationshipNode item) {
			var rel = new Relationship() {
				Id = item.Id,
				ETag = item.ETag,
				Name = item.Name,
				SourceId = Guid.Parse(item.SourceId),
				TargetId = Guid.Parse(item.TargetId),
				Properties = item.Properties
			};

			return rel;
		}
		public static IEnumerable<Relationship> MapToRelationship(this IEnumerable<RelationshipNode> items) {
			return items.Select((item) => item.MapToRelationship());
		}
		public static dynamic ToExpandoObject(this Relationship rel) {
			var eo = new ExpandoObject();
			var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
			IDictionary<string, object> dictionary = rel.Properties.UnflattenFromDotNotation();
			dictionary.Add("$relationshipId", rel.Id.ToString());
			dictionary.Add("$etag", rel.ETag);
			dictionary.Add("$sourceId", rel.SourceId);
			dictionary.Add("$targetId", rel.TargetId);
			dictionary.Add("$relationshipName", rel.Name);

			foreach (var kvp in dictionary) {
				eoColl.Add(kvp);
			}

			return eo;
		}
		public static IEnumerable<dynamic> ToExpandoObject(this IEnumerable<Relationship> rels) {
			return rels.Select(rel => rel.ToExpandoObject());
		}
		public static IDictionary<string, object> ToDotNotationDictionary(this Relationship rel) {
			IDictionary<string, object> dictionary = rel.Properties.FlattenToDotNotation();
			dictionary.Add("Id", rel.Id.ToString());
			dictionary.Add("ETag", rel.ETag);
			dictionary.Add("Name", rel.Name);
			dictionary.Add("SourceId", rel.SourceId);
			dictionary.Add("TargetId", rel.TargetId);

			return dictionary;
		}
	}
}
