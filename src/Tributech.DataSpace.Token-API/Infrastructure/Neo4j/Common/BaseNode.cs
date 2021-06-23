using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j.Common {
	public class BaseNode : INode {
		/// <summary>
		/// Label of the node
		/// </summary>
		/// <param name="label"></param>
		/// 		public BaseNode(string label) {
		public BaseNode() {
		}

		public BaseNode(string label) {
			var l = new List<string>();
			l.Add(label);
			Labels = l;
		}

		public long Id { get; set; }
		public IEnumerable<string> Labels { get; set; }

		[JsonExtensionData]
		public IDictionary<string, JToken> Data { get; set; }
	}
}
