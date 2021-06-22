using System.Net.Http;
using Microsoft.Extensions.Options;
using Tributech.DataSpace.TwinAPI.Application.Infrastructure;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI {
	public class VocabularyAPIClient: IVocabularyService {

		public VocabularyAPIClient(HttpClient httpClient, IOptions<VocabularyAPIOptions> options) {
		}
	}
}
