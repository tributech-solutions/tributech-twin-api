using System.Collections.Generic;


namespace Tributech.DataSpace.TwinAPI.Model {
	public class PaginatedResponse<T> where T : class {
		public PaginatedResponse(int totalElements, IEnumerable<T> content) {
			TotalELements = totalElements;
			Content = content;
		}

		public int TotalELements { get; }
		public IEnumerable<T> Content { get; }

	}
}
