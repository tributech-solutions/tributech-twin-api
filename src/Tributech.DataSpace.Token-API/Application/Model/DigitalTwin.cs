using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j.Common;

namespace Tributech.DataSpace.TwinAPI.Application.Model {

	public class DigitalTwin : BaseNode {

		public DigitalTwin() : base(nameof(DigitalTwin)) { }

		[JsonPropertyName("$dtId")]
		public Guid Id { get; set; }

		[JsonPropertyName("$etag")]
		public string ETag { get; set; }

		[JsonPropertyName("$metadata")]
		public DigitalTwinMetadata Metadata { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement> Properties { get; set; }

		public Dictionary<string, string> GetFlat() {
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Id", Id.ToString());
			dictionary.Add("ETag", ETag);
			dictionary.Add("ModelId", Metadata.ModelId);
			GetFlatInternal(Properties, "", dictionary);
			return dictionary;
		}

		private void GetFlatInternal(Dictionary<string, JsonElement> properties, string parentKey, in Dictionary<string, string> dict) {
			foreach (var keyValuePair in properties) {
				string key = keyValuePair.Key;
				JsonElement value = keyValuePair.Value;
				string fullkey = (null == parentKey || parentKey.Trim().Equals("")) ? key : parentKey.Trim() + "." + key;

				switch (value.ValueKind) {
					case JsonValueKind.Array:
						//this.GetFlat(dict, value.ToDictionary(), fullkey);
						break;
					case JsonValueKind.Object:
						//this.GetFlat(dict, value.ToDictionary(), fullkey);
						break;
					case JsonValueKind.Null:
					case JsonValueKind.Undefined:
						dict.Add(fullkey, null);
						break;
					case JsonValueKind.Number:
					case JsonValueKind.False:
					case JsonValueKind.True:
					case JsonValueKind.String:
					default:
						dict.Add(fullkey, value.GetString());
						break;
				}
			}
		}
	}

	public class DigitalTwinMetadata {
		[JsonPropertyName("$model")]
		public string ModelId { get; set; }

	}


}
