using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tributech.DataSpace.TwinAPI.Application.Model {

	[JsonObject()]
	public class DigitalTwin {
		[JsonProperty("$dtId")]
		public Guid Id { get; set; }

		[JsonProperty("$etag")]
		public string ETag { get; set; }

		[JsonProperty("$metadata")]
		public DigitalTwinMetadata Metadata { get; set; }

		[JsonExtensionData]
		public IDictionary<string, object> Properties;

		public IDictionary<string, object> GetFlat() {
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("Id", Id.ToString());
			dictionary.Add("ETag", ETag);
			dictionary.Add("ModelId", Metadata.ModelId);
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

		public DigitalTwin() {
			Properties = new Dictionary<string, object>();
		}
	}

	public class DigitalTwinMetadata {
		[JsonProperty("$model")]
		public string ModelId { get; set; }
	}
	public class DigitalTwinNode {
		public Guid Id { get; set; }
		public string ETag { get; set; }
		public string ModelId { get; set; }

		[JsonExtensionData]
		public IDictionary<string, object> Properties;
	}
}
