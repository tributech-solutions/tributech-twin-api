using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tributech.DSK.Twin.Core.Implementation.Api;

namespace Tributech.DataSpace.TwinAPI.Model {

	public class DigitalTwinNode {
		public Guid Id { get; set; }
		public string ETag { get; set; }
		public string ModelId { get; set; }

		[JsonExtensionData]
		public Dictionary<string, object> Properties;

		public DigitalTwinNode() {
			Properties = new Dictionary<string, object>();
		}
	}

	public static class TwinExtensions {
		public static DigitalTwin MapToDigitalTwin(this DigitalTwinNode item) {
			var twin = new DigitalTwin() {
				Id = item.Id,
				ETag = item.ETag,
				Metadata = new DigitalTwinMetadata() {
					ModelId = item?.ModelId
				},
				Properties = item.Properties
			};

			return twin;
		}
		public static IEnumerable<DigitalTwin> MapToDigitalTwin(this IEnumerable<DigitalTwinNode> items) {
			return items.Select((item) => item.MapToDigitalTwin());
		}
		public static dynamic ToExpandoObject(this DigitalTwin twin) {
			var eo = new ExpandoObject();
			var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
			IDictionary<string, object> dictionary = twin.Properties.UnflattenFromDotNotation();
			dictionary.Add("$dtId", twin.Id.ToString());
			dictionary.Add("$etag", twin.ETag);
			IDictionary<string, object> metadata = new Dictionary<string, object>();
			metadata.Add("$model", twin.Metadata.ModelId);
			dictionary.Add("$metadata", metadata);

			foreach (var kvp in dictionary) {
				eoColl.Add(kvp);
			}

			return eo;
		}
		public static IEnumerable<dynamic> ToExpandoObject(this IEnumerable<DigitalTwin> item) {
			return item.Select(t => t.ToExpandoObject());
		}
		public static IDictionary<string, object> ToDotNotationDictionary(this DigitalTwin twin) {
			IDictionary<string, object> dictionary = twin.Properties.FlattenToDotNotation();
			dictionary.Add("Id", twin.Id.ToString());
			dictionary.Add("ETag", twin.ETag);
			dictionary.Add("ModelId", twin.Metadata.ModelId);
			return dictionary;
		}
	}

	public static class DotNotationExtensions {
			public static IDictionary<string, object> UnflattenFromDotNotation(this IDictionary<string, object> dotNotation) {
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				// TODO: Handle arrays

				foreach (var dotObject in dotNotation) {
					var hierarcy = dotObject.Key.Split('.');

					Dictionary<string, object> bottom = dictionary;

					for (int i = 0; i < hierarcy.Length; i++) {
						var key = hierarcy[i];

						if (i == hierarcy.Length - 1) // Last key
						{
							bottom.Add(key, dotObject.Value);
						}
						else {
							if (!bottom.ContainsKey(key))
								bottom.Add(key, new Dictionary<string, object>());

							bottom = (Dictionary<string, object>)bottom[key];
						}
					}
				}

				return dictionary;
			}
			public static IDictionary<string, object> FlattenToDotNotation(this IDictionary<string, object> tree) {
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				GetFlatInternal(tree, "", dictionary);
				return dictionary;

				void GetFlatInternal(IDictionary<string, object> properties, string parentKey, in IDictionary<string, object> dict) {
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
}
