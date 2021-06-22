using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j.Common {
	public abstract class BaseNode : INode {

		[JsonIgnore]
		public string Label { get; private set; }

		/// <summary>
		/// Label of the node
		/// </summary>
		/// <param name="label"></param>
		public BaseNode(string label) {
			Label = label;
		}
	}
}
