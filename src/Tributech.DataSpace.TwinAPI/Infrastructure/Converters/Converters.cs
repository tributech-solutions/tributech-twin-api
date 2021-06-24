using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Application.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Converters {
	public class Converters {
		public static DigitalTwin MapToDigitalTwin(DigitalTwinNode item) {
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

		public static IEnumerable<DigitalTwin> MapToDigitalTwin(IEnumerable<DigitalTwinNode> items) {
			return items.Select((item) => MapToDigitalTwin(item));
		}
	}
}
