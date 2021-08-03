using System.Collections.Generic;


namespace Tributech.DataSpace.TwinAPI.Model {
	public class PaginatedResponse<T> where T : class {
		public PaginatedResponse(long totalElements, IEnumerable<T> content) {
			TotalElements = totalElements;
			Content = content;
		}

		public long TotalElements { get; }
		public IEnumerable<T> Content { get; }

	}
}
