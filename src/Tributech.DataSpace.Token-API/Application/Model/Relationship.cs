using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tributech.DataSpace.TwinAPI.Application.Model {
	public class Relationship {

		public Relationship() {
			Properties = new Dictionary<string, object>();
		}

		[JsonProperty("$relationshipId")]
		public Guid Id { get; set; }

		[JsonProperty("$etag")]
		public string ETag { get; set; }

		[JsonProperty("$sourceId")]
		public Guid SourceId { get; set; }

		[JsonProperty("$targetId")]
		public Guid TargetId { get; set; }

		[JsonProperty("$relationshipName")]
		public string Name { get; set; }

		[JsonExtensionData]
		public Dictionary<string, object> Properties { get; set; }

		public IDictionary<string, object> GetFlat() {
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("Id", Id.ToString());
			dictionary.Add("ETag", ETag);
			dictionary.Add("SourceId", SourceId);
			dictionary.Add("TargetId", TargetId);
			dictionary.Add("Name", Name);
			GetFlatInternal(Properties, "", dictionary);
			return dictionary;
		}

		private void GetFlatInternal(IDictionary<string, object> properties, string parentKey, in IDictionary<string, object> dict) {
			foreach (var keyValuePair in properties) {
				string key = keyValuePair.Key;
				JToken value = JToken.FromObject(keyValuePair.Value);
				string fullkey = (null == parentKey || parentKey.Trim().Length == 0) ? key : parentKey.Trim() + "." + key;

				switch (value.Type) {
					case JTokenType.Array:
						var list = JsonConvert.DeserializeObject<List<object>>(keyValuePair.Value.ToString());
						var newDict = list
							.Select((element, index) => new { element, index })
							.ToDictionary(ele => $"[{ele.index}]", ele => ele.element);
						GetFlatInternal(newDict, fullkey, dict);

						break;
					case JTokenType.Object:
						var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(keyValuePair.Value.ToString());
						GetFlatInternal(values, fullkey, dict);
						break;
					default:
						dict.Add(fullkey, keyValuePair.Value);
						break;
				}
			}
		}

	}
}
